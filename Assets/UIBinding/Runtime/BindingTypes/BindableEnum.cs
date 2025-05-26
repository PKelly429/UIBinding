using System;

namespace DataBinding
{
    /// <summary>
    /// To create a bindable enum, extend this type with the specific enum
    ///  e.g. public class MyEnumBindable : BindableVariable<MyEnum> { }
    ///
    /// GetLocalisedEnumText(object value) can be overriden to use localised text for the enum values 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BindableEnum<T> : BindableVariable<T> where T : Enum
    {
        public override string stringValue => GetLocalisedEnumText(value);
    }
}