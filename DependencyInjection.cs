using Microsoft.Extensions.DependencyInjection;
using ToDoWebAPI.Features.Commands.TaskItems.CreateTaskItem;
using ToDoWebAPI.Features.Commands.TaskItems.UpdateTaskItem;
using ToDoWebAPI.Features.Commands.TaskItems.DeleteTaskItem;
using ToDoWebAPI.Features.Queries.TaskItems.GetAllTaskItems;
using ToDoWebAPI.Features.Queries.TaskItems.GetTaskItemById;
using ToDoWebAPI.Features.Commands.Categories.CreateCategory;
using ToDoWebAPI.Features.Commands.Categories.UpdateCategory;
using ToDoWebAPI.Features.Commands.Categories.DeleteCategory;
using ToDoWebAPI.Features.Queries.Categories.GetAllCategories;
using ToDoWebAPI.Features.Queries.Categories.GetCategoryById;

namespace ToDoWebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // TaskItem Handlers
        services.AddScoped<CreateTaskItemHandler>();
        services.AddScoped<UpdateTaskItemHandler>();
        services.AddScoped<DeleteTaskItemHandler>();
        services.AddScoped<GetAllTaskItemsHandler>();
        services.AddScoped<GetTaskItemByIdHandler>();

        // Category Handlers
        services.AddScoped<CreateCategoryHandler>();
        services.AddScoped<UpdateCategoryHandler>();
        services.AddScoped<DeleteCategoryHandler>();
        services.AddScoped<GetAllCategoriesHandler>();
        services.AddScoped<GetCategoryByIdHandler>();

        return services;
    }
}
