﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.372
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rxx.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Rxx.Properties.Errors", typeof(Errors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The event member&apos;s delegate has an arguments parameter that is not compatible with System.EventArgs.  Member name: {0}..
        /// </summary>
        internal static string EventIsNotCompatibleWithEventArgs {
            get {
                return ResourceManager.GetString("EventIsNotCompatibleWithEventArgs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The event member&apos;s delegate is not compatible with System.EventHandler&lt;TEventArgs&gt;.  Member name: {0}..
        /// </summary>
        internal static string EventIsNotCompatibleWithEventHandler {
            get {
                return ResourceManager.GetString("EventIsNotCompatibleWithEventHandler", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified property does not support change events..
        /// </summary>
        internal static string PropertyDoesNotSupportChangeEvents {
            get {
                return ResourceManager.GetString("PropertyDoesNotSupportChangeEvents", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified expression contains an indexer..
        /// </summary>
        internal static string PropertyExpressionContainsIndexer {
            get {
                return ResourceManager.GetString("PropertyExpressionContainsIndexer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified expression cannot be understood as a property..
        /// </summary>
        internal static string PropertyExpressionNotUnderstood {
            get {
                return ResourceManager.GetString("PropertyExpressionNotUnderstood", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified expression cannot be understood as a property being read from a local variable, field, another property, or a chain of fields and properties..
        /// </summary>
        internal static string PropertyExpressionOwnerNotUnderstood {
            get {
                return ResourceManager.GetString("PropertyExpressionOwnerNotUnderstood", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified expression contains more than just fields and properties..
        /// </summary>
        internal static string PropertyExpressionTooComplex {
            get {
                return ResourceManager.GetString("PropertyExpressionTooComplex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The owner of the specified property could not be determined..
        /// </summary>
        internal static string PropertyExpresssionOwnerNotDetermined {
            get {
                return ResourceManager.GetString("PropertyExpresssionOwnerNotDetermined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified property is read-only.  Property name: {0}..
        /// </summary>
        internal static string PropertyIsReadOnly {
            get {
                return ResourceManager.GetString("PropertyIsReadOnly", resourceCulture);
            }
        }
    }
}
