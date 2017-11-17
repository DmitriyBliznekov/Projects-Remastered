using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;

namespace Project.Model
{
    public abstract class BaseModel : ViewModelBase, IDataErrorInfo
    {

        /// <summary>
        /// A dictionary of current errors with the name of the error-field as the key and the error
        /// text as the value.
        /// </summary>
        private static List<PropertyInfo> propertyInfos;



        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseModel()
        {
            InitCommands();
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public string Error => string.Empty;

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        public string this[string columnName]
        {
            get
            {
                CollectErrors();
                return Errors.ContainsKey(columnName) ? Errors[columnName] : string.Empty;
            }
        }

        /// <summary>
        /// Override this method in derived types to initialize command logic.
        /// </summary>
        protected virtual void InitCommands()
        {
        }

        /// <summary>
        /// Can be overridden by derived types to react on the finisihing of error-collections.
        /// </summary>
        protected virtual void OnErrorsCollected()
        {
        }

        /// <summary>
        /// Is called by the indexer to collect all errors and not only the one for a special field.
        /// </summary>
        /// <remarks>
        /// Because <see cref="HasErrors" /> depends on the <see cref="Errors" /> dictionary this
        /// ensures that controls like buttons can switch their state accordingly.
        /// </remarks>
        private void CollectErrors()
        {
            Errors.Clear();
            PropertyInfos.ForEach(
                prop =>
                {
                    var currentValue = prop.GetValue(this);
                    var requiredAttribute = prop.GetCustomAttribute<RequiredAttribute>();
                    var rangeAttribute = prop.GetCustomAttribute<RangeAttribute>();
                    var regularExpressionAttribute = prop.GetCustomAttribute<RegularExpressionAttribute>();

                    if (requiredAttribute != null)
                    {
                        if (string.IsNullOrEmpty(currentValue?.ToString() ?? string.Empty))
                        {
                            Errors.Add(prop.Name, requiredAttribute.ErrorMessage);
                        }
                    }

                    if (regularExpressionAttribute != null)
                    {
                        if (string.IsNullOrEmpty(currentValue?.ToString())) return;

                        if (!regularExpressionAttribute.IsValid(currentValue))
                        {
                            Errors.Add(prop.Name, regularExpressionAttribute.ErrorMessage);
                        }
                    }

                    if (rangeAttribute != null)
                    {
                        if (string.IsNullOrEmpty(currentValue?.ToString())) return;

                        if (regularExpressionAttribute == null ||
                            !regularExpressionAttribute.IsValid(currentValue)) return;
                        if (Convert.ToInt32(currentValue) < Convert.ToInt32(rangeAttribute.Minimum) ||
                            Convert.ToInt32(currentValue) > Convert.ToInt32(rangeAttribute.Maximum))
                        {
                            Errors.Add(prop.Name, rangeAttribute.ErrorMessage);
                        }
                    }
                });

            // we have to this because the Dictionary does not implement INotifyPropertyChanged    
            RaisePropertyChanged(nameof(HasErrors));
            RaisePropertyChanged(nameof(IsOkay));

            // commands do not recognize property changes automatically
            OnErrorsCollected();
        }

        #region Properties

        private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Indicates whether this instance has any errors.
        /// </summary>
        public bool HasErrors => Errors.Any();

        /// <summary>
        /// The opposite of <see cref="HasErrors" />.
        /// </summary>
        /// <remarks>
        /// Exists for convenient binding only.
        /// </remarks>
        public bool IsOkay => !HasErrors;

        /// <summary>
        /// Retrieves a list of all properties with attributes required for <see cref="IDataErrorInfo" /> automation.
        /// </summary>
        protected List<PropertyInfo> PropertyInfos
        {
            get
            {
                if (propertyInfos == null)
                {
                    propertyInfos = GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(prop => prop.IsDefined(typeof(RequiredAttribute), true) ||
                                       prop.IsDefined(typeof(RangeAttribute), true) ||
                                       prop.IsDefined(typeof(RegularExpressionAttribute), true))
                        .ToList();
                }
                return propertyInfos;
            }
        }
        #endregion
    }
}
