﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.372
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rxx.Labs.Properties {
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
    internal class Text {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Text() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Rxx.Labs.Properties.Text", typeof(Text).Assembly);
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
        ///   Looks up a localized string similar to Done.
        /// </summary>
        internal static string Done {
            get {
                return ResourceManager.GetString("Done", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sequence did not have any values..
        /// </summary>
        internal static string OnCompletedEmpty {
            get {
                return ResourceManager.GetString("OnCompletedEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OnCompleted: ({0:mm\:ss\.ff}).
        /// </summary>
        internal static string OnCompletedTimeFormat {
            get {
                return ResourceManager.GetString("OnCompletedTimeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OnError: ({0:mm\:ss\.ff}): {1}.
        /// </summary>
        internal static string OnErrorTimeFormat {
            get {
                return ResourceManager.GetString("OnErrorTimeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OnNext: ({0:mm\:ss\.ff}): {1}.
        /// </summary>
        internal static string OnNextTimeFormat {
            get {
                return ResourceManager.GetString("OnNextTimeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The AsOperational extension method also allows you to define binary and unary operator overloads as functions for any type, not just the primitive numeric types.  Furthermore, you can also specify a different binary strategy for combining sequences operationally (although the following example does not show that feature in particular)..
        /// </summary>
        internal static string OperationalLabAdvanced {
            get {
                return ResourceManager.GetString("OperationalLabAdvanced", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operator overload methods are combinators.  Rxx provides an AsOperational extension method that converts sequences of the primitive numeric types into sequences that can be combined using the basic C# operators.  Currently included are the binary +, -, *, / operators and the unary +, - operators, for any of the primitive numeric types in which C# implicitly defines them..
        /// </summary>
        internal static string OperationalLabBasic {
            get {
                return ResourceManager.GetString("OperationalLabBasic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}&gt; .
        /// </summary>
        internal static string PromptFormat {
            get {
                return ResourceManager.GetString("PromptFormat", resourceCulture);
            }
        }
    }
}
