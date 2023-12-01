using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._6_Routing;
using Bunit;

namespace BlazorCraft.Web.Tests._6_Routing;

[TestForPage(typeof(Pages._6_Routing.Routing))]
public class Test_Routing_Ex5_LessonFinal : RoutingTestBase<Routing_Ex5>
{
    public const string RouteById = "employee-by-id";
    public const string RouteByCompanyAccountId = "employee-by-company-account-id";

    public const string IdRouteParam = "Id";
    public const string CompanyIdRouteParam = "CompanyAccountId";
    public const string AllDataRouteParam = "AllDataShown";

    [Title("Route by Id defined")]
    [Description("This test verifies if the component has the /" + RouteById + " route defined")]
    public Task GivenRouting_Ex5_WhenDefined_ThenRouteByIdIsAccessible()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        CheckRouteByTemplate(routeAttributes, RouteById);
		return Task.CompletedTask;
	}

    [Title("Route by Id defined")]
    [Description("This test verifies if the component has the /" + RouteByCompanyAccountId + " route defined")]
    public Task GivenRouting_Ex5_WhenDefined_ThenRouteByCompanyAccountIdIsAccessible()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        CheckRouteByTemplate(routeAttributes, RouteByCompanyAccountId);
		return Task.CompletedTask;
	}

    [Title("Route parameters")]
    [Description("This test verifies if the route parameters are defined with the correct name and type")]
    public Task GivenRouteParameters_WhenDefined_ThenHaveCorrectNameAndType()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        var idAttribute = CheckRouteByTemplate(routeAttributes, RouteById);
        var companyIdAttribute = CheckRouteByTemplate(routeAttributes, RouteByCompanyAccountId);

        var idParam = FindRouteParameterByName(Component, IdRouteParam);
        ValidateRouteParameterType(idParam, typeof(int?));
        ValidateRouteParameterConstraint(idAttribute, IdRouteParam, "int?");

        var companyIdParam = FindRouteParameterByName(Component, CompanyIdRouteParam);
        ValidateRouteParameterType(companyIdParam, typeof(string));
        ValidateRouteParameterConstraint(companyIdAttribute, IdRouteParam, "?", true);

        var idShowAllParam = FindRouteParameterByName(Component, AllDataRouteParam);
        ValidateRouteParameterType(idShowAllParam, typeof(bool));
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, idAttribute);
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, companyIdAttribute);
		return Task.CompletedTask;
	}

    [Title("Id with all data")]
    [Description("This test verifies if company id is given with all data shown, the component renders accordingly")]
    public Task GivenCompanyIdWithAllData_WhenRendered_ThenComponentRendersAccordingly()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        var idAttribute = CheckRouteByTemplate(routeAttributes, RouteById);
        var companyIdAttribute = CheckRouteByTemplate(routeAttributes, RouteByCompanyAccountId);

        var idParam = FindRouteParameterByName(Component, IdRouteParam);
        ValidateRouteParameterType(idParam, typeof(int?));
        ValidateRouteParameterConstraint(idAttribute, IdRouteParam, "int?");

        var companyIdParam = FindRouteParameterByName(Component, CompanyIdRouteParam);
        ValidateRouteParameterType(companyIdParam, typeof(string));
        ValidateRouteParameterConstraint(companyIdAttribute, IdRouteParam, "?", true);

        var idShowAllParam = FindRouteParameterByName(Component, AllDataRouteParam);
        ValidateRouteParameterType(idShowAllParam, typeof(bool));
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, idAttribute);
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, companyIdAttribute);

        TestContext testContext = new TestContext();
        int? paramValue = 2;
        bool showAllValue = true;
        var renderedComponent = testContext.RenderComponent<Routing_Ex5>(new[]
        {
            ComponentParameter.CreateParameter(IdRouteParam, paramValue),
            ComponentParameter.CreateParameter(AllDataRouteParam, showAllValue),
        });
        var renderedComponentMarkup = renderedComponent.Markup;
        var expectedOutput =
            $"<p>Employee id: 2, Company assigned identifier: bbbb, Name: Test Thomas, Position: CFO</p>";
        AssertCorrectMarkup(expectedOutput, renderedComponentMarkup);
		return Task.CompletedTask;
	}

    [Title("Id with NOT ALL data")]
    [Description(
        "This test verifies if company id is given with not all data shown, the component renders accordingly")]
    public Task GivenCompanyIdWithNotAllData_WhenRendered_ThenComponentRendersAccordingly()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        var idAttribute = CheckRouteByTemplate(routeAttributes, RouteById);
        var companyIdAttribute = CheckRouteByTemplate(routeAttributes, RouteByCompanyAccountId);

        var idParam = FindRouteParameterByName(Component, IdRouteParam);
        ValidateRouteParameterType(idParam, typeof(int?));
        ValidateRouteParameterConstraint(idAttribute, IdRouteParam, "int?");

        var companyIdParam = FindRouteParameterByName(Component, CompanyIdRouteParam);
        ValidateRouteParameterType(companyIdParam, typeof(string));
        ValidateRouteParameterConstraint(companyIdAttribute, IdRouteParam, "?", true);

        var idShowAllParam = FindRouteParameterByName(Component, AllDataRouteParam);
        ValidateRouteParameterType(idShowAllParam, typeof(bool));
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, idAttribute);
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, companyIdAttribute);

        TestContext testContext = new TestContext();
        int? paramValue = 2;
        bool showAllValue = false;
        var renderedComponent = testContext.RenderComponent<Routing_Ex5>(new[]
        {
            ComponentParameter.CreateParameter(IdRouteParam, paramValue),
            ComponentParameter.CreateParameter(AllDataRouteParam, showAllValue),
        });
        var renderedComponentMarkup = renderedComponent.Markup;
        var expectedOutput = $"<p>Test Thomas</p>";
        AssertCorrectMarkup(expectedOutput, renderedComponentMarkup);
		return Task.CompletedTask;
	}

    [Title("Company Account Id with all data")]
    [Description("This test verifies if company id is given with all data shown, the component renders accordingly")]
    public Task GivenCompanyAccountIdWithAllData_WhenRendered_ThenComponentRendersAccordingly()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        var idAttribute = CheckRouteByTemplate(routeAttributes, RouteById);
        var companyIdAttribute = CheckRouteByTemplate(routeAttributes, RouteByCompanyAccountId);

        var idParam = FindRouteParameterByName(Component, IdRouteParam);
        ValidateRouteParameterType(idParam, typeof(int?));
        ValidateRouteParameterConstraint(idAttribute, IdRouteParam, "int?");

        var companyIdParam = FindRouteParameterByName(Component, CompanyIdRouteParam);
        ValidateRouteParameterType(companyIdParam, typeof(string));
        ValidateRouteParameterConstraint(companyIdAttribute, IdRouteParam, "?", true);

        var idShowAllParam = FindRouteParameterByName(Component, AllDataRouteParam);
        ValidateRouteParameterType(idShowAllParam, typeof(bool));
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, idAttribute);
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, companyIdAttribute);

        TestContext testContext = new TestContext();
        string paramValue = "bbbb";
        bool showAllValue = true;
        var renderedComponent = testContext.RenderComponent<Routing_Ex5>(new[]
        {
            ComponentParameter.CreateParameter(CompanyIdRouteParam, paramValue),
            ComponentParameter.CreateParameter(AllDataRouteParam, showAllValue),
        });
        var renderedComponentMarkup = renderedComponent.Markup;
        var expectedOutput =
            $"<p>Employee id: 2, Company assigned identifier: bbbb, Name: Test Thomas, Position: CFO</p>";
        AssertCorrectMarkup(expectedOutput, renderedComponentMarkup);
		return Task.CompletedTask;
	}

    [Title("Company Account Id with NOT ALL data")]
    [Description(
        "This test verifies if company id is given with not all data shown, the component renders accordingly")]
    public Task GivenCompanyAccountIdWithNotAllData_WhenRendered_ThenComponentRendersAccordingly()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        var idAttribute = CheckRouteByTemplate(routeAttributes, RouteById);
        var companyIdAttribute = CheckRouteByTemplate(routeAttributes, RouteByCompanyAccountId);

        var idParam = FindRouteParameterByName(Component, IdRouteParam);
        ValidateRouteParameterType(idParam, typeof(int?));
        ValidateRouteParameterConstraint(idAttribute, IdRouteParam, "int?");

        var companyIdParam = FindRouteParameterByName(Component, CompanyIdRouteParam);
        ValidateRouteParameterType(companyIdParam, typeof(string));
        ValidateRouteParameterConstraint(companyIdAttribute, IdRouteParam, "?", true);

        var idShowAllParam = FindRouteParameterByName(Component, AllDataRouteParam);
        ValidateRouteParameterType(idShowAllParam, typeof(bool));
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, idAttribute);
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, companyIdAttribute);

        TestContext testContext = new TestContext();
        string paramValue = "bbbb";
        bool showAllValue = false;
        var renderedComponent = testContext.RenderComponent<Routing_Ex5>(new[]
        {
            ComponentParameter.CreateParameter(CompanyIdRouteParam, paramValue),
            ComponentParameter.CreateParameter(AllDataRouteParam, showAllValue),
        });
        var renderedComponentMarkup = renderedComponent.Markup;
        var expectedOutput = $"<p>Test Thomas</p>";
        AssertCorrectMarkup(expectedOutput, renderedComponentMarkup);
		return Task.CompletedTask;
	}

    [Title("No Id passed")]
    [Description(
        "This test verifies if company id is given with not all data shown, the component renders accordingly")]
    public Task GivenNoIdPassed_WhenRendered_ThenComponentRendersAccordingly()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        var idAttribute = CheckRouteByTemplate(routeAttributes, RouteById);
        var companyIdAttribute = CheckRouteByTemplate(routeAttributes, RouteByCompanyAccountId);

        var idParam = FindRouteParameterByName(Component, IdRouteParam);
        ValidateRouteParameterType(idParam, typeof(int?));
        ValidateRouteParameterConstraint(idAttribute, IdRouteParam, "int?");

        var companyIdParam = FindRouteParameterByName(Component, CompanyIdRouteParam);
        ValidateRouteParameterType(companyIdParam, typeof(string));
        ValidateRouteParameterConstraint(companyIdAttribute, IdRouteParam, "?", true);

        var idShowAllParam = FindRouteParameterByName(Component, AllDataRouteParam);
        ValidateRouteParameterType(idShowAllParam, typeof(bool));
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, idAttribute);
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, companyIdAttribute);

        TestContext testContext = new TestContext();
        bool showAllValue = false;
        var renderedComponent = testContext.RenderComponent<Routing_Ex5>(new[]
        {
            ComponentParameter.CreateParameter(AllDataRouteParam, showAllValue),
        });
        var renderedComponentMarkup = renderedComponent.Markup;
        var expectedOutput = $"<p>No identifier received</p>";
        AssertCorrectMarkup(expectedOutput, renderedComponentMarkup);
		return Task.CompletedTask;
	}

    [Title("Not found by Id")]
    [Description("This test verifies if id is given, but the employee is not found, the component renders accordingly")]
    public Task GivenNotFoundById_WhenRendered_ThenComponentRendersAccordingly()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        var idAttribute = CheckRouteByTemplate(routeAttributes, RouteById);
        var companyIdAttribute = CheckRouteByTemplate(routeAttributes, RouteByCompanyAccountId);

        var idParam = FindRouteParameterByName(Component, IdRouteParam);
        ValidateRouteParameterType(idParam, typeof(int?));
        ValidateRouteParameterConstraint(idAttribute, IdRouteParam, "int?");

        var companyIdParam = FindRouteParameterByName(Component, CompanyIdRouteParam);
        ValidateRouteParameterType(companyIdParam, typeof(string));
        ValidateRouteParameterConstraint(companyIdAttribute, IdRouteParam, "?", true);

        var idShowAllParam = FindRouteParameterByName(Component, AllDataRouteParam);
        ValidateRouteParameterType(idShowAllParam, typeof(bool));
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, idAttribute);
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, companyIdAttribute);

        TestContext testContext = new TestContext();
        bool showAllValue = false;
        var renderedComponent = testContext.RenderComponent<Routing_Ex5>(new[]
        {
            ComponentParameter.CreateParameter(IdRouteParam, 9999999),
            ComponentParameter.CreateParameter(AllDataRouteParam, showAllValue),
        });
        var renderedComponentMarkup = renderedComponent.Markup;
        var expectedOutput = $"<p>Employee not found</p>";
        AssertCorrectMarkup(expectedOutput, renderedComponentMarkup);
		return Task.CompletedTask;
	}

    [Title("Not found by Company Account Id")]
    [Description(
        "This test verifies if Company Account Id is given, but the employee is not found, the component renders accordingly")]
    public Task GivenNotFoundByCompanyAccountId_WhenRendered_ThenComponentRendersAccordingly()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        var idAttribute = CheckRouteByTemplate(routeAttributes, RouteById);
        var companyIdAttribute = CheckRouteByTemplate(routeAttributes, RouteByCompanyAccountId);

        var idParam = FindRouteParameterByName(Component, IdRouteParam);
        ValidateRouteParameterType(idParam, typeof(int?));
        ValidateRouteParameterConstraint(idAttribute, IdRouteParam, "int?");

        var companyIdParam = FindRouteParameterByName(Component, CompanyIdRouteParam);
        ValidateRouteParameterType(companyIdParam, typeof(string));
        ValidateRouteParameterConstraint(companyIdAttribute, IdRouteParam, "?", true);

        var idShowAllParam = FindRouteParameterByName(Component, AllDataRouteParam);
        ValidateRouteParameterType(idShowAllParam, typeof(bool));
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, idAttribute);
        FindRouteParameterByNameInRoute(Component, AllDataRouteParam, companyIdAttribute);

        TestContext testContext = new TestContext();
        bool showAllValue = false;
        var renderedComponent = testContext.RenderComponent<Routing_Ex5>(new[]
        {
            ComponentParameter.CreateParameter(CompanyIdRouteParam, "xyz"),
            ComponentParameter.CreateParameter(AllDataRouteParam, showAllValue),
        });
        var renderedComponentMarkup = renderedComponent.Markup;
        var expectedOutput = $"<p>Employee not found</p>";
        AssertCorrectMarkup(expectedOutput, renderedComponentMarkup);
		return Task.CompletedTask;
	}
}