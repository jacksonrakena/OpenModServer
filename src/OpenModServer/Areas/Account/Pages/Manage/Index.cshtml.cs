// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenModServer.Data;
using OpenModServer.Data.Identity;

namespace OpenModServer.Areas.Account.Pages.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<OmsUser> _userManager;
        private readonly SignInManager<OmsUser> _signInManager;
        private readonly ApplicationDbContext _database;
        
        public IndexModel(
            UserManager<OmsUser> userManager,
            SignInManager<OmsUser> signInManager,
            ApplicationDbContext db)
        {
            _database = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            [DataType(DataType.Url)]
            [Display(Name = "Website")]
            [MaxLength(128)]
            public string? Website { get; set; }
            [MaxLength(128)]
            
            public string? FacebookPageName { get; set; }
            [MaxLength(128)]
            public string? TwitterUsername { get; set; }
            [MaxLength(128)]
            public string? SteamCommunityName { get; set; }
            [MaxLength(128)]
            public string? GitHubName { get; set; }
            [MaxLength(128)]
            public string? DiscordInviteCode { get; set; }
            
            [MaxLength(12)]
            [Display(Name = "Country")]
            public string? CountryIsoCode { get; set; }
            [MaxLength(128)]
            public string? City { get; set; }
        }

        private async Task LoadAsync(OmsUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Website = user.Website,
                TwitterUsername = user.TwitterUsername,
                GitHubName = user.GitHubName,
                DiscordInviteCode = user.DiscordInviteCode,
                FacebookPageName = user.FacebookPageName,
                SteamCommunityName = user.SteamCommunityName,
                City = user.City,
                CountryIsoCode = user.CountryIsoCode
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            _database.Users.Attach(user);
            
            user.GitHubName = this.Input.GitHubName;
            user.Website = this.Input.Website;
            user.TwitterUsername = Input.TwitterUsername;
            user.CountryIsoCode = Input.CountryIsoCode;
            user.City = Input.City;
            
            await _database.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
