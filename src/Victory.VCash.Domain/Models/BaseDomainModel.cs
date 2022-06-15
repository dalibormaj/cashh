using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Models
{
    public abstract class BaseDomainModel
    {
        /// <summary>
        /// Apply new (NOT NULL) values to the model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newModel">Model to apply</param>
        public T ApplyNewValues<T>(T newModel) where T : BaseDomainModel
        {
            var applyNewValuesMethod =
                (
                    from m in GetType().GetMethods(BindingFlags.Static | BindingFlags.Public)
                    where m.Name == nameof(ApplyNewValues)
                    let p = m.GetParameters()
                    where p.Length == 2
                        && p[0].ParameterType.Name.Equals(nameof(T))
                        && p[1].ParameterType.Name.Equals(nameof(T))
                    select m
                ).FirstOrDefault();


            var propertyInfos = typeof(T).GetProperties();

            if (newModel != null)
            {
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    var currentValue = typeof(T).GetProperty(propertyInfo.Name).GetValue(this, null);
                    var newValue = typeof(T).GetProperty(propertyInfo.Name).GetValue(newModel, null);

                    if (newValue != null)
                    {
                        var newValueType = newValue.GetType();

                        if ((newValueType.IsValueType || newValueType == typeof(string)))
                        {
                            propertyInfo.SetValue(this, newValue);
                            continue;
                        }
                        if (newValue is BaseDomainModel)
                        {
                            var itemType = newValue.GetType();
                            var applyNewValuesGenericMethod = applyNewValuesMethod.MakeGenericMethod(itemType);
                            applyNewValuesGenericMethod.Invoke(null, new object[] { currentValue, newValue });
                            continue;
                        }

                        if (newValue is ICollection && (newValue as ICollection).Count > 0)
                        {
                            var currentValueArray = new ArrayList((ICollection)currentValue);
                            var newValueArray = new ArrayList((ICollection)newValue);

                            if (currentValueArray.Count != newValueArray.Count)
                                throw new Exception("ApplyNewValues cannot be used for collections with different size. Please manually set the values");

                            //go through all elements to apply 
                            for (var i = 0; i < newValueArray.Count; i++)
                            {
                                var itemType = newValueArray[i].GetType();
                                var applyNewValuesGenericMethod = applyNewValuesMethod.MakeGenericMethod(itemType);

                                //apply new values object per object
                                applyNewValuesGenericMethod.Invoke(null, new object[] { currentValueArray[i], newValueArray[i] });
                            }

                            continue;
                        }
                    }
                }
            }

            return (T)this;
        }
    }
}
