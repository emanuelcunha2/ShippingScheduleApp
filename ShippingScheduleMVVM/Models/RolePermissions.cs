using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.ViewModels;
using ShippingScheduleMVVM.Views.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShippingScheduleMVVM.Models
{
    public class RolePermissions
    {
        public static bool CanDragRecord(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];
            
            if (role == "Admin"){ return true; }

            if (role == "COPS" & recordCategory == "Created"){ return true; }

            if (role == "Supervisor" & (recordCategory == "Prepared" || recordCategory == "Released" || recordCategory == "Process")){ return true; }

            return false;
        }

        public static bool CanSetCategoryCreated(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if (role == "Admin") { return true; }

            if (role == "COPS" & recordCategory == "Created") { return true; }

            if (role == "Shipping" & recordCategory != "Shipped") { return true; }

            if (role == "Supervisor" & recordCategory == "Created") { return true; }
            return false;
        }

        public static bool CanSetCategoryReleased(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if (role == "Admin") { return true; }

            if (role == "COPS" & (recordCategory == "Created" || recordCategory == "Released") ) { return true; }

            if (role == "Shipping" & recordCategory == "Released") { return true; }

            if (role == "Supervisor" & recordCategory == "Released") { return true; }

            return false;
        }
        public static bool CanSetCategoryHandled(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if (role == "Admin") { return true; }

            if (role == "COPS" & (recordCategory == "Released" || recordCategory == "Created")) { return true; }

            if (role == "COPS" & recordCategory == "Process") { return true; }

            if (role == "Shipping" & (recordCategory == "Process" || recordCategory == "Released")) { return true; }

            if (role == "Supervisor" & (recordCategory == "Process" || recordCategory == "Released")) { return true; }

            return false;
        }

        public static bool CanSetCategoryPrepared(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if (role == "Admin") { return true; }

            if (role == "COPS" & recordCategory == "Prepared") { return true; }

            if (role == "Shipping" & (recordCategory == "Process" || recordCategory == "Prepared")) { return true; }

            if (role == "Supervisor" & (recordCategory == "Process" || recordCategory == "Prepared")) { return true; }

            return false;
        }
        public static bool CanSetCategoryShipped(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if (role == "Admin") { return true; }

            if (role == "COPS" & recordCategory == "Shipped") { return true; }

            if (role == "Shipping" & (recordCategory == "Shipped" || recordCategory == "Prepared")) { return true; }

            if (role == "Supervisor" & (recordCategory == "Shipped" || recordCategory == "Prepared")) { return true; }

            return false;
        }
        public static bool CanSetTypeOfShipment(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if (role == "Admin") { return true; }

            if ((role == "COPS" || role == "Shipping" || role == "Supervisor") & recordCategory == "Created") { return true; }

            return false;
        }
        public static bool CanSetTransportMode(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if (role == "Admin") { return true; }

            if ((role == "COPS" || role == "Shipping" || role == "Supervisor") & recordCategory == "Created") { return true; }

            return false;
        }
        public static bool CanSetCarrier(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if (role == "Admin") { return true; }

            if ((role == "COPS" || role == "Shipping" || role == "Supervisor") & recordCategory == "Created") { return true; }

            return false;
        }
        public static bool ClientNotificationChangesReleased(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if ((role == "COPS" ) & (recordCategory == "Released" || recordCategory == "Created")) { return true; }

            return false;
        }
        public static bool ClientNotificationChangesCreated(string recordCategory)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if ((role == "COPS") & recordCategory == "Created") { return true; }

            return false;
        }
        // não alterar para 0 e N/A
        public static bool AllRequirementsMetHandled(ObservableCollection<Part> parts, string recordCategory, ViewModelBase parentViewModel)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];
            if(recordCategory != "Process") { return true; }   
            if (role == "Admin") { return true; }

            if(role != "Admin")
            {
                bool foundEmptyDeliveryNote = false;
                foreach(Part part in parts)
                {
                    if (part.FinalQuantity == 0 && part.DeliveryNote.IsNullOrEmpty()) { part.DeliveryNote = "N/A"; }
                    if (part.DeliveryNote.IsNullOrEmpty()) { foundEmptyDeliveryNote = true; break; }                   
                }
                if(foundEmptyDeliveryNote) 
                {
                    System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == parentViewModel);
                    if (owner == null) { return false; }

                    var window = new CustomAlertWindow("Erro Handled", "Delivery notes incompletas", parentViewModel);
                    window.Owner = owner;

                    window.ShowDialog();
                    return false;
                }
                return true;
            }
            return false;
        }
        public static bool AllRequirementsMetShipped(ObservableCollection<Part> parts, string recordCategory, ViewModelBase parentViewModel)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];
            if (recordCategory != "Shipped") { return true; }
            if (role == "Admin") { return true; }

            if (role != "Admin")
            {
                bool foundEmptyQuantity = false;
                foreach (Part part in parts)
                {
                    if ((part.FinalQuantity == 0) && part.DeliveryNote.IsNullOrEmpty()) { foundEmptyQuantity = true; }
                }
                if (foundEmptyQuantity)
                {
                    System.Windows.Window? owner = Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == parentViewModel);
                    if (owner == null) { return false; }

                    var window = new CustomAlertWindow("Erro Shipped", "Delivery Notes Incompletas", parentViewModel);
                    window.Owner = owner;

                    window.ShowDialog();
                    return false;
                }
                return true;
            }
            return false;
        }
        public static bool CanSaveRecordChanges(bool hasRecordShipped, string previousCategory, string currentCategory, Visibility toBeHandledBlockVisibility)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];

            if (previousCategory != "Shipped")
            {
                if (previousCategory == currentCategory)
                    return true;
            }

            if((role == "Supervisor" || role == "Shipping") && currentCategory == "Released") { return true; }
            if((role == "Supervisor" || role == "Shipping") && previousCategory == "Created" && (currentCategory == "Created" || currentCategory == "Released")) { return false; }
            
            if (role != "Admin" && hasRecordShipped)
            {
                return false;
            }

            if(role == "COPS" && (currentCategory == "Prepared" || currentCategory == "Shipped"))
            {
                return false;
            }


            return true;
        }
        public static bool CanDeleteRecord(string recordCategory, bool isCreatingOrUpdatingCreated)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];
            if (role != "Admin" && !isCreatingOrUpdatingCreated)
            {
                return false;
            }
            return true;
        }
        public static void ChangePartsAfterUpdating(string recordCategory, ObservableCollection<Part> parts, RecordWindowViewModel record)
        {
            /*
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];
            // Reset
            foreach (Part part in parts)
            {
                part.ShipToEnabled= true;
                part.ApnEnabled= true;
                part.CpnEnabled= true;
                part.DesignationEnabled= true;
                part.UnloadingPointEnabled= true;
                part.ExpectedQuantityEnabled= true;
                part.FinalQuantityEnabled= true;
                part.DeliveryNoteEnabled= true;
                part.TransportNumberEnabled= true;
                part.FinalQuantityEnabled = true;
            }

            if (role == "Admin") { return; }

            // Notifications and transport arrived
            if(record.SelectedRecord.Category == "Created") 
            { 
                record.IsTransportArrivedChangesEnabled = false;
            }
            else { record.IsNotificationChangesEnabled = false; }

            if(role == "COPS")
            {
                foreach (Part part in parts)
                {
                    part.FinalQuantityEnabled = false;
                }
            }

            if (recordCategory != "Created" && recordCategory != "Released" && role == "COPS")
            {
                foreach (Part part in parts)
                {
                    part.ShipToEnabled = false;
                    part.ApnEnabled = false;
                    part.CpnEnabled = false;
                    part.DesignationEnabled = false;
                    part.UnloadingPointEnabled = false;
                    part.ExpectedQuantityEnabled = false;
                    part.DeliveryNoteEnabled = false;
                    part.TransportNumberEnabled = false;
                }
            }

            if (recordCategory == "Created" && (role == "Shipping" || role == "Supervisor"))
            {
                foreach (Part part in parts)
                {
                    part.ShipToEnabled = false;
                    part.ApnEnabled = false;
                    part.CpnEnabled = false;
                    part.DesignationEnabled = false;
                    part.UnloadingPointEnabled = false;
                    part.ExpectedQuantityEnabled = false;
                    part.FinalQuantityEnabled = false;
                    part.DeliveryNoteEnabled = false;
                    part.TransportNumberEnabled = false;
                    part.FinalQuantityEnabled = false;
                }
            }

            if (recordCategory != "Created" && (role == "Shipping" || role == "Supervisor"))
            {
                foreach (Part part in parts)
                {
                    part.ShipToEnabled = false;
                    part.ApnEnabled = false;
                    part.CpnEnabled = false;
                    part.DesignationEnabled = false;
                    part.UnloadingPointEnabled = false;
                    part.ExpectedQuantityEnabled = false; 
                }
            }

            */
        }
        public static bool SetProjectPermissions(int userId, string name)
        {
            return true;
            /*
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];
            if(role == "Admin")
            {
                return true;
            }
            else
            {
                if (userId != App.loggedUser?.Id || name == "Geral")
                {
                    return false;
                }
                return true;
            } 
            */
        }

        public static bool AreSpecialSettingsEnabled()
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];
            if (role == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CanAlterPartsTables(string category)
        {
            string role = (App.loggedUser?.Roles?[0] == null) ? "" : App.loggedUser.Roles[0];
            if (role == "Admin")
            {
                return true;
            }

            if(category == "Prepared" || category == "Shipped" || category == "Process")
            {
                return false;
            }
            return true;
        }
    }
}
