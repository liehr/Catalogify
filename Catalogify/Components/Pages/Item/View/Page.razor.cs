using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using DBFactory = Microsoft.EntityFrameworkCore.IDbContextFactory<Catalogify.Data.ApplicationDbContext>;
namespace Catalogify.Components.Pages.Item.View;

public partial class Page
{
    [Parameter]
    public Guid Id { get; set; }
    
    private Item? Model { get; set; }
    
    [Inject]
    private DBFactory? DbFactory { get; set; }
    
    [Inject]
    private ToastService? ToastService { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        if (DbFactory is null)
        {
            ToastService?.Notify(new ToastMessage
            {
                Title = "Error",
                Type = ToastType.Danger,
                Message = "The item could not be loaded. Something on our end went wrong.",
                HelpText = "Try again later!",
                IconName = IconName.ExclamationTriangleFill,
                AutoHide = true
            });
            return;
        }
        
        Model = await Data.GetItemAsync(Id, Guid.Empty, DbFactory);
    }
}