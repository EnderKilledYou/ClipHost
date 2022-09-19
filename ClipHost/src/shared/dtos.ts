/* Options:
Date: 2022-09-18 23:53:29
Version: 6.21
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: https://localhost:5001

//GlobalNamespace: 
//MakePropertiesOptional: False
//AddServiceStackTypes: True
//AddResponseStatus: False
//AddImplicitVersion: 
//AddDescriptionAsComments: True
//IncludeTypes: 
//ExcludeTypes: 
//DefaultImports: 
*/


export interface IReturn<T>
{
    createResponse(): T;
}

export interface IReturnVoid
{
    createResponse(): void;
}

export interface IHasSessionId
{
    SessionId: string;
}

export interface IHasBearerToken
{
    BearerToken: string;
}

export interface IPost
{
}

export class TablesUp implements ITableUp
{
    public Id: number;

    public constructor(init?: Partial<TablesUp>) { (Object as any).assign(this, init); }
}

export class Streamer extends TablesUp
{
    public Id: number;
    // @Required()
    public Name: string;

    public constructor(init?: Partial<Streamer>) { super(init); (Object as any).assign(this, init); }
}

export class StreamerCommandCenter extends TablesUp
{
    public Id: number;
    // @Required()
    // @References("typeof(ClipHost.ServiceModel.Streamer)")
    public StreamerId: number;

    // @Required()
    // @References("typeof(ClipHost.ServiceModel.CommandCenter)")
    public CommandCenterId: number;

    public constructor(init?: Partial<StreamerCommandCenter>) { super(init); (Object as any).assign(this, init); }
}

export class CommandCenter extends TablesUp
{
    public Name: string;
    public StreamerCount: number;
    public MaxStreamers: number;

    public constructor(init?: Partial<CommandCenter>) { super(init); (Object as any).assign(this, init); }
}

export class ProgramInstance implements IHaveBlazorConnection, IProgramInstance
{

    public constructor(init?: Partial<ProgramInstance>) { (Object as any).assign(this, init); }
}

export class DtoProgramInstance extends ProgramInstance
{
    public DtoId?: number;
    public ReportsArray: QueueReport[];

    public constructor(init?: Partial<DtoProgramInstance>) { super(init); (Object as any).assign(this, init); }
}

export class QueueReport
{
    public Id: number;
    public Size: number;
    public MaxSize: number;
    public AverageSeconds: number;
    public HighSeconds: number;
    public Low: number;
    public Name: string;
    public ProcessId: number;

    public constructor(init?: Partial<QueueReport>) { (Object as any).assign(this, init); }
}

// @DataContract
export class ResponseError
{
    // @DataMember(Order=1)
    public ErrorCode: string;

    // @DataMember(Order=2)
    public FieldName: string;

    // @DataMember(Order=3)
    public Message: string;

    // @DataMember(Order=4)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<ResponseError>) { (Object as any).assign(this, init); }
}

// @DataContract
export class ResponseStatus
{
    // @DataMember(Order=1)
    public ErrorCode: string;

    // @DataMember(Order=2)
    public Message: string;

    // @DataMember(Order=3)
    public StackTrace: string;

    // @DataMember(Order=4)
    public Errors: ResponseError[];

    // @DataMember(Order=5)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<ResponseStatus>) { (Object as any).assign(this, init); }
}

export interface ITableUp
{
}

export interface IHaveBlazorConnection
{
}

export interface IProgramInstance
{
}

export class HelloTestResponse
{
    public Result: string;

    public constructor(init?: Partial<HelloTestResponse>) { (Object as any).assign(this, init); }
}

export class HelloResponse
{
    public Result: string;

    public constructor(init?: Partial<HelloResponse>) { (Object as any).assign(this, init); }
}

export class ListDtoProgramInstanceResponse
{
    public Count: number;
    public Message: string;
    public Success: boolean;
    public DtoProgramInstances: DtoProgramInstance[];

    public constructor(init?: Partial<ListDtoProgramInstanceResponse>) { (Object as any).assign(this, init); }
}

export class ListStreamerResponse
{
    public Count: number;
    public Message: string;
    public Success: boolean;
    public Streamers: Streamer[];

    public constructor(init?: Partial<ListStreamerResponse>) { (Object as any).assign(this, init); }
}

export class CreateStreamerResponse
{
    public Id: number;
    public Message: string;
    public Success: boolean;

    public constructor(init?: Partial<CreateStreamerResponse>) { (Object as any).assign(this, init); }
}

export class CreateStreamerCommandCenterResponse
{
    public Id: number;
    public Message: string;
    public Success: boolean;

    public constructor(init?: Partial<CreateStreamerCommandCenterResponse>) { (Object as any).assign(this, init); }
}

export class ListQueueReportResponse
{
    public Count: number;
    public Message: string;
    public Success: boolean;
    public QueueReports: QueueReport[];

    public constructor(init?: Partial<ListQueueReportResponse>) { (Object as any).assign(this, init); }
}

export class CreateCommandCenterResponse
{
    public Id: number;
    public Message: string;
    public Success: boolean;

    public constructor(init?: Partial<CreateCommandCenterResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class AuthenticateResponse implements IHasSessionId, IHasBearerToken
{
    // @DataMember(Order=1)
    public UserId: string;

    // @DataMember(Order=2)
    public SessionId: string;

    // @DataMember(Order=3)
    public UserName: string;

    // @DataMember(Order=4)
    public DisplayName: string;

    // @DataMember(Order=5)
    public ReferrerUrl: string;

    // @DataMember(Order=6)
    public BearerToken: string;

    // @DataMember(Order=7)
    public RefreshToken: string;

    // @DataMember(Order=8)
    public ProfileUrl: string;

    // @DataMember(Order=9)
    public Roles: string[];

    // @DataMember(Order=10)
    public Permissions: string[];

    // @DataMember(Order=11)
    public ResponseStatus: ResponseStatus;

    // @DataMember(Order=12)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<AuthenticateResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class AssignRolesResponse
{
    // @DataMember(Order=1)
    public AllRoles: string[];

    // @DataMember(Order=2)
    public AllPermissions: string[];

    // @DataMember(Order=3)
    public Meta: { [index: string]: string; };

    // @DataMember(Order=4)
    public ResponseStatus: ResponseStatus;

    public constructor(init?: Partial<AssignRolesResponse>) { (Object as any).assign(this, init); }
}

// @DataContract
export class UnAssignRolesResponse
{
    // @DataMember(Order=1)
    public AllRoles: string[];

    // @DataMember(Order=2)
    public AllPermissions: string[];

    // @DataMember(Order=3)
    public Meta: { [index: string]: string; };

    // @DataMember(Order=4)
    public ResponseStatus: ResponseStatus;

    public constructor(init?: Partial<UnAssignRolesResponse>) { (Object as any).assign(this, init); }
}

// @Route("/test")
// @Route("/test/{Name}")
export class HelloTest implements IReturn<HelloTestResponse>
{
    public Name: string;

    public constructor(init?: Partial<HelloTest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'HelloTest'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new HelloTestResponse(); }
}

// @Route("/hello")
// @Route("/hello/{Name}")
export class Hello implements IReturn<HelloResponse>
{
    public Name: string;

    public constructor(init?: Partial<Hello>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'Hello'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new HelloResponse(); }
}

export class ListDtoProgramInstanceRequest implements IReturn<ListDtoProgramInstanceResponse>
{
    public After: number;

    public constructor(init?: Partial<ListDtoProgramInstanceRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'ListDtoProgramInstanceRequest'; }
    public getMethod() { return 'GET'; }
    public createResponse() { return new ListDtoProgramInstanceResponse(); }
}

export class ListStreamerRequest implements IReturn<ListStreamerResponse>
{
    public After: number;
    public Name: string;

    public constructor(init?: Partial<ListStreamerRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'ListStreamerRequest'; }
    public getMethod() { return 'GET'; }
    public createResponse() { return new ListStreamerResponse(); }
}

export class CreateStreamerRequest implements IReturn<CreateStreamerResponse>
{
    public Streamer: Streamer;

    public constructor(init?: Partial<CreateStreamerRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'CreateStreamerRequest'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new CreateStreamerResponse(); }
}

export class CreateStreamerCommandCenterRequest implements IReturn<CreateStreamerCommandCenterResponse>
{
    public StreamerCommandCenter: StreamerCommandCenter;

    public constructor(init?: Partial<CreateStreamerCommandCenterRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'CreateStreamerCommandCenterRequest'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new CreateStreamerCommandCenterResponse(); }
}

export class ListQueueReportRequest implements IReturn<ListQueueReportResponse>
{
    public After: number;
    public Name: string;

    public constructor(init?: Partial<ListQueueReportRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'ListQueueReportRequest'; }
    public getMethod() { return 'GET'; }
    public createResponse() { return new ListQueueReportResponse(); }
}

export class CreateCommandCenterRequest implements IReturn<CreateCommandCenterResponse>
{
    public CommandCenter: CommandCenter;

    public constructor(init?: Partial<CreateCommandCenterRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'CreateCommandCenterRequest'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new CreateCommandCenterResponse(); }
}

/**
* Sign In
*/
// @Route("/auth", "OPTIONS,GET,POST,DELETE")
// @Route("/auth/{provider}", "OPTIONS,GET,POST,DELETE")
// @Api(Description="Sign In")
// @DataContract
export class Authenticate implements IReturn<AuthenticateResponse>, IPost
{
    /**
    * AuthProvider, e.g. credentials
    */
    // @DataMember(Order=1)
    public provider: string;

    // @DataMember(Order=2)
    public State: string;

    // @DataMember(Order=3)
    public oauth_token: string;

    // @DataMember(Order=4)
    public oauth_verifier: string;

    // @DataMember(Order=5)
    public UserName: string;

    // @DataMember(Order=6)
    public Password: string;

    // @DataMember(Order=7)
    public RememberMe?: boolean;

    // @DataMember(Order=9)
    public ErrorView: string;

    // @DataMember(Order=10)
    public nonce: string;

    // @DataMember(Order=11)
    public uri: string;

    // @DataMember(Order=12)
    public response: string;

    // @DataMember(Order=13)
    public qop: string;

    // @DataMember(Order=14)
    public nc: string;

    // @DataMember(Order=15)
    public cnonce: string;

    // @DataMember(Order=17)
    public AccessToken: string;

    // @DataMember(Order=18)
    public AccessTokenSecret: string;

    // @DataMember(Order=19)
    public scope: string;

    // @DataMember(Order=20)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<Authenticate>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'Authenticate'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new AuthenticateResponse(); }
}

// @Route("/assignroles", "POST")
// @DataContract
export class AssignRoles implements IReturn<AssignRolesResponse>, IPost
{
    // @DataMember(Order=1)
    public UserName: string;

    // @DataMember(Order=2)
    public Permissions: string[];

    // @DataMember(Order=3)
    public Roles: string[];

    // @DataMember(Order=4)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<AssignRoles>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'AssignRoles'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new AssignRolesResponse(); }
}

// @Route("/unassignroles", "POST")
// @DataContract
export class UnAssignRoles implements IReturn<UnAssignRolesResponse>, IPost
{
    // @DataMember(Order=1)
    public UserName: string;

    // @DataMember(Order=2)
    public Permissions: string[];

    // @DataMember(Order=3)
    public Roles: string[];

    // @DataMember(Order=4)
    public Meta: { [index: string]: string; };

    public constructor(init?: Partial<UnAssignRoles>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'UnAssignRoles'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new UnAssignRolesResponse(); }
}

