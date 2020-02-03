using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.SegmentAnalytics.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AdminAntiForgery]
    public class WidgetsSegmentAnalyticsController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public WidgetsSegmentAnalyticsController(
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods
        
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var segmentAnalyticsSettings = _settingService.LoadSetting<SegmentAnalyticsSettings>(storeScope);

            var model = new ConfigurationModel
            {
                SegmentId = segmentAnalyticsSettings.SegmentId,
                TrackingScript = segmentAnalyticsSettings.TrackingScript,
                IncludingTax = segmentAnalyticsSettings.IncludingTax,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.SegmentId_OverrideForStore = _settingService.SettingExists(segmentAnalyticsSettings, x => x.SegmentId, storeScope);
                model.TrackingScript_OverrideForStore = _settingService.SettingExists(segmentAnalyticsSettings, x => x.TrackingScript, storeScope);
                model.IncludingTax_OverrideForStore = _settingService.SettingExists(segmentAnalyticsSettings, x => x.IncludingTax, storeScope);
            }

            return View("~/Plugins/Widgets.SegmentAnalytics/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var segmentAnalyticsSettings = _settingService.LoadSetting<SegmentAnalyticsSettings>(storeScope);

            segmentAnalyticsSettings.SegmentId = model.SegmentId;
            segmentAnalyticsSettings.TrackingScript = model.TrackingScript;
            segmentAnalyticsSettings.IncludingTax = model.IncludingTax;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(segmentAnalyticsSettings, x => x.SegmentId, model.SegmentId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(segmentAnalyticsSettings, x => x.TrackingScript, model.TrackingScript_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(segmentAnalyticsSettings, x => x.IncludingTax, model.IncludingTax_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        #endregion
    }
}