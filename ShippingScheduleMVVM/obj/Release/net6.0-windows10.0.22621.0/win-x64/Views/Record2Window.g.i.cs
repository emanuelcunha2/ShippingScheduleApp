﻿#pragma checksum "..\..\..\..\..\Views\Record2Window.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "69460C218FC908DBFAD5FFD0C032766107A305DA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using Microsoft.Xaml.Behaviors.Input;
using Microsoft.Xaml.Behaviors.Layout;
using Microsoft.Xaml.Behaviors.Media;
using ShippingScheduleMVVM.Behaviours;
using ShippingScheduleMVVM.Converters;
using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.ViewModels;
using ShippingScheduleMVVM.Views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ShippingScheduleMVVM.Views {
    
    
    /// <summary>
    /// Record2Window
    /// </summary>
    public partial class Record2Window : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 20 "..\..\..\..\..\Views\Record2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ShippingScheduleMVVM.Views.Record2Window ThisRecord2Window;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\..\Views\Record2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\..\Views\Record2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridRecord;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\..\..\Views\Record2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ProjectsListview;
        
        #line default
        #line hidden
        
        
        #line 398 "..\..\..\..\..\Views\Record2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridParts;
        
        #line default
        #line hidden
        
        
        #line 487 "..\..\..\..\..\Views\Record2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView PartsListview;
        
        #line default
        #line hidden
        
        
        #line 768 "..\..\..\..\..\Views\Record2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ExtRetImg;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ShippingScheduleMVVM;component/views/record2window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\Record2Window.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ThisRecord2Window = ((ShippingScheduleMVVM.Views.Record2Window)(target));
            return;
            case 2:
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.GridRecord = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.ProjectsListview = ((System.Windows.Controls.ListView)(target));
            
            #line 93 "..\..\..\..\..\Views\Record2Window.xaml"
            this.ProjectsListview.PreviewMouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.ListView_PreviewMouseWheel);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 284 "..\..\..\..\..\Views\Record2Window.xaml"
            ((System.Windows.Controls.Border)(target)).IsEnabledChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.Border_IsEnabledChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 305 "..\..\..\..\..\Views\Record2Window.xaml"
            ((System.Windows.Controls.Border)(target)).IsEnabledChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.Border_IsEnabledChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 326 "..\..\..\..\..\Views\Record2Window.xaml"
            ((System.Windows.Controls.Border)(target)).IsEnabledChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.Border_IsEnabledChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 347 "..\..\..\..\..\Views\Record2Window.xaml"
            ((System.Windows.Controls.Border)(target)).IsEnabledChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.Border_IsEnabledChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 368 "..\..\..\..\..\Views\Record2Window.xaml"
            ((System.Windows.Controls.Border)(target)).IsEnabledChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.Border_IsEnabledChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.GridParts = ((System.Windows.Controls.Grid)(target));
            return;
            case 11:
            
            #line 405 "..\..\..\..\..\Views\Record2Window.xaml"
            ((System.Windows.Controls.ListView)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListView_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            this.PartsListview = ((System.Windows.Controls.ListView)(target));
            
            #line 487 "..\..\..\..\..\Views\Record2Window.xaml"
            this.PartsListview.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListView_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 17:
            this.ExtRetImg = ((System.Windows.Controls.Image)(target));
            
            #line 768 "..\..\..\..\..\Views\Record2Window.xaml"
            this.ExtRetImg.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ExtendRetractTable);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 13:
            
            #line 542 "..\..\..\..\..\Views\Record2Window.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            break;
            case 14:
            
            #line 602 "..\..\..\..\..\Views\Record2Window.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            break;
            case 15:
            
            #line 615 "..\..\..\..\..\Views\Record2Window.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            break;
            case 16:
            
            #line 628 "..\..\..\..\..\Views\Record2Window.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

