using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Navigation;
using Traffix.Mapping.Services;

namespace Traffix
{

    public partial class App : Application
    {

        // Avoid double-initialization
        private bool _phoneApplicationInitialized = false;
        

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {

            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            Behaviors.ViewModelResolverBehavior.LookupAssemblyNames.Add("Traffix");
            InitializeResolver();

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();
            
        }


        #region public

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        #endregion

        #region private

        #region Application Events

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {

            //i do not handle tombstoning the device here, although you could
            //this is where that code would go to resume state

        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            Services.ApplicationSettingsService.Save();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {

            //i do not handle tombstoning the device here, although you could
            //this is where that code would go to save state
            Services.ApplicationSettingsService.Save();

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }
       
        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }

        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }

        }   

        #endregion

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {

            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
            {
                RootVisual = RootFrame;
            }

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;

        }

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {

            if (this._phoneApplicationInitialized)
            {
                return;
            }

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            this._phoneApplicationInitialized = true;

        }

        private void InitializeResolver()
        {

            //register singleton settings
            DependencyResolver.Register(typeof(ViewModels.ShellViewModel), new ViewModels.ShellViewModel());

            //we register a mock service here since the device geolocation chip does not work on the emulator
            //for deployment you would change to the default service for the phone
            DependencyResolver.Register(typeof(IGeoPositionWatcher<GeoCoordinate>), new MockGeoCoordinateWatcherService());
            DependencyResolver.Register(typeof(LocationService), new LocationService());

            //we could write a service to manage multipe providers but we have to consider how that would impact
            //phone performance, battery usage and memory
            DependencyResolver.Register(typeof(Traffix.Mapping.Entities.EntityProvider), new EntityProviders.BingTrafficEntityProvider());
            DependencyResolver.Register(typeof(ViewModels.MapViewModel), null);

        }

        #endregion

    }

}