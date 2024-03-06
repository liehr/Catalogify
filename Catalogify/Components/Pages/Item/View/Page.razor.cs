using System.Drawing;
using System.Drawing.Imaging;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QRCoder;
using DBFactory = Microsoft.EntityFrameworkCore.IDbContextFactory<Catalogify.Data.ApplicationDbContext>;

namespace Catalogify.Components.Pages.Item.View
{
    public partial class Page
    {
        [Parameter]
        public Guid Id { get; set; }
        
        private Item? Model { get; set; }
        
        [Inject]
        private DBFactory? DbFactory { get; set; }
        
        [Inject]
        private ToastService? ToastService { get; set; }
        
        [Inject]
        private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
        
        [Inject]
        private NavigationManager? NavigationManager { get; set; }
        
        private string QrByte { get; set; } = string.Empty;
        
        protected override async Task OnInitializedAsync()
        {
            if (DbFactory is null || AuthenticationStateProvider is null || NavigationManager is null)
            {
                ShowErrorToast("The item could not be loaded. Something on our end went wrong.", "Try again later!");
                return;
            }
            
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var idClaim = authState.User.Claims.FirstOrDefault(e => e.Type == "Id");
            
            if (idClaim is null)
            {
                ShowErrorToast("The item could not be loaded. Something on our end went wrong.", "Try again later!");
                return;
            }
            
            Model = await Data.GetItemAsync(Id, Guid.Parse(idClaim.Value), DbFactory);
            
            if (Model is null)
            {
                ShowErrorToast("You do not have access to this item. It may have been deleted or you may not have permission to view it. An Email has been sent to the owner.", "Not Authorized!");
                NavigationManager.NavigateTo("/Inventory/List");
            }

            using MemoryStream ms = new();
            QRCodeGenerator qrCodeGenerate = new();
            var qrCodeData = qrCodeGenerate.CreateQrCode(Model.Id.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new(qrCodeData);
            using var qrBitMap = qrCode.GetGraphic(5);
            qrBitMap.Save(ms, ImageFormat.Png);
            var base64 = Convert.ToBase64String(ms.ToArray());
            QrByte = $"data:image/png;base64,{base64}";
            
            await base.OnInitializedAsync();
        }

        private void ShowErrorToast(string message, string helpText)
        {
            ToastService?.Notify(new ToastMessage
            {
                Title = "Error",
                Type = ToastType.Danger,
                Message = message,
                HelpText = helpText,
                IconName = IconName.ExclamationTriangleFill,
                AutoHide = true
            });
        }
    }
}