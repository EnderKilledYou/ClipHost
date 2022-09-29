/* Options:
Date: 2022-09-27 15:50:37
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
    // @Required()
    public Name: string;

    public Enabled: boolean;

    public constructor(init?: Partial<Streamer>) { super(init); (Object as any).assign(this, init); }
}

export class StreamerCommandCenter extends TablesUp
{
    // @Required()
    // @References("typeof(ClipHost.ServiceModel.Streamer)")
    public StreamerId: number;

    // @Required()
    // @References("typeof(ClipHost.ServiceModel.CommandCenter)")
    public CommandCenterId: number;

    public constructor(init?: Partial<StreamerCommandCenter>) { super(init); (Object as any).assign(this, init); }
}

export class ProcessReport extends TablesUp
{
    public IsRunning: boolean;
    public ExitCode: number;
    public ReportText: string;
    public ProcessId: number;
    // @Required()
    // @References("typeof(ClipHost.ServiceModel.StreamerCommandCenter)")
    public StreamerCommandCenterId: number;

    public constructor(init?: Partial<ProcessReport>) { super(init); (Object as any).assign(this, init); }
}

export class CommandCenter extends TablesUp
{
    public Name: string;
    public StreamerCount: number;
    public MaxStreamers: number;

    public constructor(init?: Partial<CommandCenter>) { super(init); (Object as any).assign(this, init); }
}

export class CommandCenterReport extends TablesUp implements IQueueReport
{
    public Name: string;
    public TotalProcessed: number;
    public AverageMilliSeconds: number;
    public HighMilliSeconds: number;
    public LowMilliSeconds: number;
    public MaxSize: number;
    public _processId: number;
    public ProcessId: number;
    public Size: number;
    // @Required()
    // @References("typeof(ClipHost.ServiceModel.StreamerCommandCenter)")
    public StreamerCommandCenterId: number;

    public constructor(init?: Partial<CommandCenterReport>) { super(init); (Object as any).assign(this, init); }
}

export class ProgramInstance implements IHaveBlazorConnection, IProgramInstance, IReportInstance
{
    public ReportsArray: QueueReport[];

    public constructor(init?: Partial<ProgramInstance>) { (Object as any).assign(this, init); }
}

export class DtoProgramInstance extends ProgramInstance implements IDtoProgramInstance
{
    public DtoId?: number;
    public ReportsArray: QueueReport[];

    public constructor(init?: Partial<DtoProgramInstance>) { super(init); (Object as any).assign(this, init); }
}

export class Tuple_3<T1, T2, T3>
{
    public Item1: T1;
    public Item2: T2;
    public Item3: T3;

    public constructor(init?: Partial<Tuple_3<T1, T2, T3>>) { (Object as any).assign(this, init); }
}

export class QueueReport implements IQueueReport
{
    public Id: number;
    public Size: number;
    public MaxSize: number;
    public AverageMilliSeconds: number;
    public HighMilliSeconds: number;
    public LowMilliSeconds: number;
    public Name: string;
    public ProcessId: number;

    public constructor(init?: Partial<QueueReport>) { (Object as any).assign(this, init); }
}

export class Tuple_4<T1, T2, T3, T4>
{
    public Item1: T1;
    public Item2: T2;
    public Item3: T3;
    public Item4: T4;

    public constructor(init?: Partial<Tuple_4<T1, T2, T3, T4>>) { (Object as any).assign(this, init); }
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

export interface IQueueReport
{
    AverageMilliSeconds: number;
    HighMilliSeconds: number;
    Id: number;
    LowMilliSeconds: number;
    MaxSize: number;
    Name: string;
    ProcessId: number;
    Size: number;
}

export interface IHaveBlazorConnection
{
}

export interface IProgramInstance
{
}

export interface IReportInstance
{
    ReportsArray: QueueReport[];
}

export interface IDtoProgramInstance
{
    DtoId?: number;
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

export class DeleteStreamerResponse
{
    public Message: string;
    public Success: boolean;
    public DeletedStreamer: Streamer;

    public constructor(init?: Partial<DeleteStreamerResponse>) { (Object as any).assign(this, init); }
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

export class ListStreamerCommandCenterResponse
{
    public Count: number;
    public Message: string;
    public Success: boolean;
    public StreamerCommandCenters: Tuple_3<StreamerCommandCenter,Streamer,CommandCenter>[];

    public constructor(init?: Partial<ListStreamerCommandCenterResponse>) { (Object as any).assign(this, init); }
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

export class ListProcessReportResponse
{
    public Count: number;
    public Message: string;
    public Success: boolean;
    public ProcessReports: Tuple_4<StreamerCommandCenter,ProcessReport,Streamer,CommandCenter>[];

    public constructor(init?: Partial<ListProcessReportResponse>) { (Object as any).assign(this, init); }
}

export class CreateProcessReportResponse
{
    public Id: number;
    public Message: string;
    public Success: boolean;
    public ResponseStatus: ResponseStatus;

    public constructor(init?: Partial<CreateProcessReportResponse>) { (Object as any).assign(this, init); }
}

export class ListCommandCenterResponse
{
    public Count: number;
    public Message: string;
    public Success: boolean;
    public CommandCenters: CommandCenter[];

    public constructor(init?: Partial<ListCommandCenterResponse>) { (Object as any).assign(this, init); }
}

export class CreateCommandCenterResponse
{
    public Id: number;
    public Message: string;
    public Success: boolean;

    public constructor(init?: Partial<CreateCommandCenterResponse>) { (Object as any).assign(this, init); }
}

export class ListCommandCenterReportResponse
{
    public Count: number;
    public Message: string;
    public Success: boolean;
    public CommandCenterReports: CommandCenterReport[];

    public constructor(init?: Partial<ListCommandCenterReportResponse>) { (Object as any).assign(this, init); }
}

export class CreateCommandCenterReportResponse
{
    public Id: number;
    public Message: string;
    public Success: boolean;
    public ResponseStatus: ResponseStatus;

    public constructor(init?: Partial<CreateCommandCenterReportResponse>) { (Object as any).assign(this, init); }
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

export class DeleteStreamerRequest implements IReturn<DeleteStreamerResponse>
{
    public Id: number;

    public constructor(init?: Partial<DeleteStreamerRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'DeleteStreamerRequest'; }
    public getMethod() { return 'DELETE'; }
    public createResponse() { return new DeleteStreamerResponse(); }
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

export class ListStreamerCommandCenterRequest implements IReturn<ListStreamerCommandCenterResponse>
{
    public After: number;

    public constructor(init?: Partial<ListStreamerCommandCenterRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'ListStreamerCommandCenterRequest'; }
    public getMethod() { return 'GET'; }
    public createResponse() { return new ListStreamerCommandCenterResponse(); }
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
    public getMethod() { return 'POST'; }
    public createResponse() { return new ListQueueReportResponse(); }
}

export class ListProcessReportRequest implements IReturn<ListProcessReportResponse>
{
    public After: number;
    public IsRunning?: boolean;
    public ProcessId: number;

    public constructor(init?: Partial<ListProcessReportRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'ListProcessReportRequest'; }
    public getMethod() { return 'GET'; }
    public createResponse() { return new ListProcessReportResponse(); }
}

export class CreateProcessReportRequest implements IReturn<CreateProcessReportResponse>
{
    public ProcessReport: ProcessReport;

    public constructor(init?: Partial<CreateProcessReportRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'CreateProcessReportRequest'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new CreateProcessReportResponse(); }
}

export class ListCommandCenterRequest implements IReturn<ListCommandCenterResponse>
{
    public After: number;

    public constructor(init?: Partial<ListCommandCenterRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'ListCommandCenterRequest'; }
    public getMethod() { return 'GET'; }
    public createResponse() { return new ListCommandCenterResponse(); }
}

export class CreateCommandCenterRequest implements IReturn<CreateCommandCenterResponse>
{
    public CommandCenter: CommandCenter;

    public constructor(init?: Partial<CreateCommandCenterRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'CreateCommandCenterRequest'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new CreateCommandCenterResponse(); }
}

export class ListCommandCenterReportRequest implements IReturn<ListCommandCenterReportResponse>
{
    public After: number;

    public constructor(init?: Partial<ListCommandCenterReportRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'ListCommandCenterReportRequest'; }
    public getMethod() { return 'GET'; }
    public createResponse() { return new ListCommandCenterReportResponse(); }
}

export class CreateCommandCenterReportRequest implements IReturn<CreateCommandCenterReportResponse>
{
    public CommandCenterReport: CommandCenterReport;

    public constructor(init?: Partial<CreateCommandCenterReportRequest>) { (Object as any).assign(this, init); }
    public getTypeName() { return 'CreateCommandCenterReportRequest'; }
    public getMethod() { return 'POST'; }
    public createResponse() { return new CreateCommandCenterReportResponse(); }
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

