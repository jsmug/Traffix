using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Traffix.Behaviors
{
    
    public static class ViewModelResolverBehavior
    {

        private static readonly List<string> _lookupAssemblies = new List<string>();


        public static readonly DependencyProperty HasViewModelProperty = DependencyProperty.RegisterAttached("HasViewModel", typeof(bool), typeof(ViewModelResolverBehavior), new PropertyMetadata(false, OnHasViewModelChanged));


        #region public

        public static bool GetHasViewModel(UserControl target)
        {

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            return (bool)target.GetValue(HasViewModelProperty);

        }

        public static ICollection<string> LookupAssemblyNames
        {

            get
            {
                return _lookupAssemblies;
            }

        }

        public static void SetHasViewModel(UserControl target, bool value)
        {
            
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            target.SetValue(HasViewModelProperty, value);

        }

        #endregion

        #region private

        private static IEnumerable<Assembly> GetLookupAssemblies()
        {

            List<Assembly> currentAssemblies = new List<Assembly>() { Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly() };
            IEnumerable<Assembly> assemblies = (from string name in _lookupAssemblies
                                                from Assembly assembly in currentAssemblies
                                                where string.Compare(assembly.GetType().Assembly.FullName.Substring(0, assembly.GetType().Assembly.FullName.IndexOf(",")), name, StringComparison.Ordinal) == 0
                                                select assembly);

            return assemblies != null && assemblies.Count() > 0 ? assemblies : currentAssemblies.AsEnumerable(); 

        }

        private static object GetViewModel(string viewModelName)
        {

            IEnumerable<Assembly> assemblies = GetLookupAssemblies();
            object vm = null;

            foreach (Assembly assembly in assemblies)
            {
                
                foreach (Type type in assembly.GetExportedTypes())
                {

                    System.Diagnostics.Debug.WriteLine(type.FullName);

                    if (string.Compare(type.Name, viewModelName, StringComparison.Ordinal) == 0)
                    {

                        vm = DependencyResolver.Resolve(type);
                        break;

                    }

                }

                if (vm != null)
                {
                    break;
                }

            }

            return vm;

        }

        private static void OnHasViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            UserControl uc = d as UserControl;
            string namebuffer = default(string);
            string viewModelName = default(string);

            if (uc != null)
            {

                if((bool)e.NewValue)
                {

                    namebuffer = uc.GetType().Name;

                    if(!namebuffer.EndsWith("View"))
                    {
                        namebuffer += "View";
                    }

                    viewModelName = namebuffer + "Model";
                    uc.DataContext = GetViewModel(viewModelName);

                }
                else
                {
                    uc.DataContext = null;
                }

            }

        }

        #endregion

    }

}
