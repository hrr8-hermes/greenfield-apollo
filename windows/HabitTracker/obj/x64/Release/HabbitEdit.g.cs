﻿#pragma checksum "C:\Users\mcjob\Documents\Visual Studio 2015\Projects\HabitTracker\HabitTracker\HabbitEdit.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C1B93B5C34D65737F77F8F4C75122D19"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HabitTracker
{
    partial class HabbitEdit : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.HabitName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 2:
                {
                    this.RemindAt = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3:
                {
                    this.DueAt = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4:
                {
                    this.update = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 18 "..\..\..\HabbitEdit.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.update).Click += this.update_Click;
                    #line default
                }
                break;
            case 5:
                {
                    this.deactivate = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 19 "..\..\..\HabbitEdit.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.deactivate).Click += this.deactivate_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.habitName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 7:
                {
                    this.remind = (global::Windows.UI.Xaml.Controls.TimePicker)(target);
                }
                break;
            case 8:
                {
                    this.due = (global::Windows.UI.Xaml.Controls.TimePicker)(target);
                }
                break;
            case 9:
                {
                    this.button = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 23 "..\..\..\HabbitEdit.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.button).Click += this.button_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

