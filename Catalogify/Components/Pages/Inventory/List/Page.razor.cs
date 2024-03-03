using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using DBFactory = Microsoft.EntityFrameworkCore.IDbContextFactory<Catalogify.Data.ApplicationDbContext>;

namespace Catalogify.Components.Pages.Inventory.List;

public partial class Page
{
    [Inject]
    private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
    
    [Inject]
    private DBFactory? DbFactory { get; set; }
    
    [Inject]
    private ToastService? ToastService { get; set; }
    
    private List<Inventory>? _inventories;

    private string? _userName;

    private readonly List<Tuple<string, string>> _colors =
    [
        new Tuple<string, string>("bg-primary", "text-white"),
        new Tuple<string, string>("bg-primary-subtle", "text-primary-emphasis"),
        new Tuple<string, string>("bg-secondary", "text-white"),
        new Tuple<string, string>("bg-secondary-subtle", "text-secondary-emphasis"),
        new Tuple<string, string>("bg-success", "text-white"),
        new Tuple<string, string>("bg-danger", "text-white"),
        new Tuple<string, string>("bg-warning", "text-dark"),
        new Tuple<string, string>("bg-info", "text-white"),
        new Tuple<string, string>("bg-light", "text-dark"),
        new Tuple<string, string>("bg-light-subtle", "text-dark-emphasis"),
        new Tuple<string, string>("bg-dark", "text-white"),
        new Tuple<string, string>("bg-dark-subtle", "text-dark-emphasis")
    ];

    private readonly Dictionary<string, string> _outlineColors = new()
    {
        { "bg-primary", "btn-outline-light" },
        { "bg-primary-subtle", "btn-outline-dark" },
        { "bg-secondary", "btn-outline-light" },
        { "bg-secondary-subtle", "btn-outline-dark" },
        { "bg-success", "btn-outline-light" },
        { "bg-danger", "btn-outline-light" },
        { "bg-warning", "btn-outline-dark"},
        { "bg-info", "btn-outline-dark" },
        { "bg-light", "btn-outline-dark" },
        { "bg-light-subtle", "btn-outline-dark" },
        { "bg-dark", "btn-outline-light" },
        { "bg-dark-subtle", "btn-outline-dark" }
    };

    private readonly Dictionary<string, string> _borderColors = new()
    {
        { "bg-primary", "border-primary" },
        { "bg-primary-subtle", "border-primary" },
        { "bg-secondary", "border-secondary" },
        { "bg-secondary-subtle", "border-secondary" },
        { "bg-success", "border-success" },
        { "bg-danger", "border-danger" },
        { "bg-warning", "border-warning"},
        { "bg-info", "border-info" },
        { "bg-light", "border-secondary" },
        { "bg-light-subtle", "border-secondary" },
        { "bg-dark", "border-dark" },
        { "bg-dark-subtle", "border-dark" }
    };
    protected override async Task OnInitializedAsync()
    {
        await GetInventories();
        
        if (DbFactory is null || AuthenticationStateProvider is null)
        {
            ToastService?.Notify(new ToastMessage
            {
                Title = "Error",
                Type = ToastType.Danger,
                Message = "Your inventories could not be loaded. Something on our end went wrong.",
                HelpText = "Try again later!",
                IconName = IconName.ExclamationTriangleFill,
                AutoHide = true
            });
        }
    }
    
    private async Task GetInventories()
    {
        var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();
        var idClaim = authState.User.Claims.FirstOrDefault(e => e.Type == "Id");
        _userName = authState.User.Identity?.Name;
        _inventories = await Data.GetInventoriesAsync(Guid.Parse(idClaim!.Value), DbFactory!);
    }

    private async Task OnCreateEmptyInventory()
    {
        var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();
        var idClaim = authState.User.Claims.FirstOrDefault(e => e.Type == "Id");
        
        var inventory = new Catalogify.Data.Entities.Inventory
        {
            Name = "My awesome new inventory",
            Description = "This is a new inventory created by Catalogify. Enjoy!",
            OwnerId = Guid.Parse(idClaim!.Value),
            ItemCount = 0,
            Metadata = new Dictionary<string, string>(),
            CreatedAt = DateTime.UtcNow,
            LastModifiedAt = DateTime.UtcNow
        };
        
        var (bgColor, textColor) = PickRandomColor();
        var outlineColor = GetOutlineColor(bgColor);
        var borderColor = GetBorderColor(bgColor);
        
        inventory.Metadata.Add("bgColor", bgColor);
        inventory.Metadata.Add("textColor", textColor);
        inventory.Metadata.Add("outlineColor", outlineColor);
        inventory.Metadata.Add("borderColor", borderColor);
        
        await using var dbContext = await DbFactory!.CreateDbContextAsync();
        dbContext.Inventories.Add(inventory);
        await dbContext.SaveChangesAsync();
        
        await GetInventories();
    }

    private Tuple<string, string> PickRandomColor()
    {
        var random = new Random();
        return _colors[random.Next(0, _colors.Count)];
    }

    private string GetOutlineColor(string color)
    {
        return _outlineColors[color];
    }
    
    private string GetBorderColor(string color)
    {
        return _borderColors[color];
    }
}