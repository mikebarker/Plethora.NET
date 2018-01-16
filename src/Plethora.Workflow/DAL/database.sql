/*
CREATE SCHEMA work;
GO
*/

IF EXISTS(SELECT 1 FROM sys.types WHERE user_type_id = TYPE_ID('dbo.IntList') AND is_table_type = 1)
    DROP TYPE dbo.IntList;
GO

CREATE TYPE dbo.IntList AS TABLE
(
    Value                   INT             NOT NULL
);
GO


IF EXISTS(SELECT 1 FROM sys.types WHERE user_type_id = TYPE_ID('dbo.KeyValueList') AND is_table_type = 1)
    DROP TYPE dbo.KeyValueList;
GO

CREATE TYPE dbo.KeyValueList AS TABLE
(
    [Key]                   VARCHAR(50)     NOT NULL,
    Value                   VARCHAR(8000)   NOT NULL
);
GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.FK_BusinessStateTransition_PrevFinalBusinessStateId') AND parent_object_id = OBJECT_ID('work.BusinessStateTransition') AND type = 'F')
    ALTER TABLE work.BusinessStateTransition
    DROP CONSTRAINT FK_BusinessStateTransition_PrevFinalBusinessStateId;

IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.FK_BusinessStateTransition_NextInitialBusinessStateId') AND parent_object_id = OBJECT_ID('work.BusinessStateTransition') AND type = 'F')
    ALTER TABLE work.BusinessStateTransition
    DROP CONSTRAINT FK_BusinessStateTransition_NextInitialBusinessStateId;

IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.FK_WorkItem_WorkflowId') AND parent_object_id = OBJECT_ID('work.WorkItem') AND type = 'F')
    ALTER TABLE work.WorkItem
    DROP CONSTRAINT FK_WorkItem_WorkflowId;

IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.FK_WorkItem_SystemStatusId') AND parent_object_id = OBJECT_ID('work.WorkItem') AND type = 'F')
    ALTER TABLE work.WorkItem
    DROP CONSTRAINT FK_WorkItem_SystemStatusId;

IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.FK_WorkItem_InitialBusinessStateId') AND parent_object_id = OBJECT_ID('work.WorkItem') AND type = 'F')
    ALTER TABLE work.WorkItem
    DROP CONSTRAINT FK_WorkItem_InitialBusinessStateId;

IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.FK_WorkItem_FinalBusinessStateId') AND parent_object_id = OBJECT_ID('work.WorkItem') AND type = 'F')
    ALTER TABLE work.WorkItem
    DROP CONSTRAINT FK_WorkItem_FinalBusinessStateId;

GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.SystemStatus') AND type = 'U')
    DROP TABLE work.SystemStatus;
GO

CREATE TABLE work.SystemStatus
(
    SystemStatusId          INT                 PRIMARY KEY,
    Name                    VARCHAR(50)         NOT NULL,
);

GO


TRUNCATE TABLE work.SystemStatus;

INSERT INTO work.SystemStatus
    (SystemStatusId, Name)
VALUES
    (0, 'Created'),
    (1, 'Ready'),
    (2, 'Processing'),
    (3, 'Timed-out'),
    (4, 'Rescheduled'),
    (5, 'Failed'),
    (6, 'Complete'),
    (7, 'Cancelled')
GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.BusinessState') AND type = 'U')
    DROP TABLE work.BusinessState;
GO

CREATE TABLE work.BusinessState
(
    BusinessStateId             INT                 PRIMARY KEY IDENTITY(1,1),
    [Name]					    VARCHAR(50)         NOT NULL,
	TimeoutMillisecond          INT                 NULL,
    MaxRetryCount               INT                 NULL,
    CustomSelectFunc            VARCHAR(255)        NULL,
    [Description]               VARCHAR(MAX)        NULL,
);
GO

CREATE UNIQUE NONCLUSTERED INDEX IX__BusinessState__UniqueName
    ON  work.BusinessState ([Name]);
GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.BusinessStateTransition') AND type = 'U')
    DROP TABLE work.BusinessStateTransition;
GO

CREATE TABLE work.BusinessStateTransition
(
    BusinessStateTransitionId   INT                 IDENTITY(1,1),
    PrevFinalBusinessStateId    INT                 NOT NULL,
    NextInitialBusinessStateId  INT                 NOT NULL,
    [Description]               VARCHAR(MAX)        NULL,

    CONSTRAINT PK_BusinessStateTransition PRIMARY KEY NONCLUSTERED (BusinessStateTransitionId),
);
GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.Workflow') AND type = 'U')
    DROP TABLE work.Workflow;
GO

CREATE TABLE work.Workflow
(
    WorkflowId              BIGINT              PRIMARY KEY IDENTITY(1,1),
    ExternalId              VARCHAR(3000)       NULL,
    [Description]           VARCHAR(MAX)        NULL,
);

GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.WorkItem') AND type = 'U')
    DROP TABLE work.WorkItem;
GO

CREATE TABLE work.WorkItem
(
    WorkItemId              BIGINT              PRIMARY KEY IDENTITY(1,1),
    WorkflowId              BIGINT              NOT NULL,
    [Priority]              INT                 NOT NULL DEFAULT(0),
    SystemStatusId          INT                 NOT NULL,
    [Sequence]              VARCHAR(8000)       NOT NULL,
    InitialBusinessStateId  INT                 NOT NULL,
    FinalBusinessStateId    INT                 NULL,
    LockTimeoutUtc          DATETIME            NULL,
    LockSystemId            UNIQUEIDENTIFIER    NULL,
    RetryCount              INT                 NOT NULL DEFAULT(0),
	DataId                  BIGINT              NULL,
	DataVersion             INT                 NULL
);
GO

CREATE NONCLUSTERED INDEX IX__WorkItem__TryGetWork
    ON  work.WorkItem (SystemStatusId, InitialBusinessStateId, [Priority] DESC, WorkItemId ASC);
GO

CREATE NONCLUSTERED INDEX IX__WorkItem__Timeout
    ON  work.WorkItem (LockTimeoutUtc)
    INCLUDE (SystemStatusId)
    WHERE SystemStatusId = 2;
GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.WorkItemData') AND type = 'U')
    DROP TABLE work.WorkItemData;
GO

CREATE TABLE work.WorkItemData
(
    DataId                  BIGINT              NOT NULL,
    DataVersion             INT                 NOT NULL,
    [Key]                   VARCHAR(50)         NOT NULL,
    [Value]                 VARCHAR(8000)       NOT NULL,

    CONSTRAINT PK_WorkItemData PRIMARY KEY NONCLUSTERED (DataId, [Key])
);
GO

CREATE CLUSTERED INDEX IX__WorkItemData__TryGetWork
    ON  work.WorkItemData (DataId, DataVersion);
GO


ALTER TABLE work.BusinessStateTransition
ADD CONSTRAINT FK_BusinessStateTransition_PrevFinalBusinessStateId FOREIGN KEY (PrevFinalBusinessStateId)
        REFERENCES work.BusinessState (BusinessStateId);

ALTER TABLE work.BusinessStateTransition
ADD CONSTRAINT FK_BusinessStateTransition_NextInitialBusinessStateId FOREIGN KEY (NextInitialBusinessStateId)
        REFERENCES work.BusinessState (BusinessStateId);

ALTER TABLE work.WorkItem
ADD CONSTRAINT FK_WorkItem_WorkflowId FOREIGN KEY (WorkflowId)
        REFERENCES work.Workflow (WorkflowId);

ALTER TABLE work.WorkItem
ADD CONSTRAINT FK_WorkItem_SystemStatusId FOREIGN KEY (SystemStatusId)
        REFERENCES work.SystemStatus (SystemStatusId);

ALTER TABLE work.WorkItem
ADD CONSTRAINT FK_WorkItem_InitialBusinessStateId FOREIGN KEY (InitialBusinessStateId)
        REFERENCES work.BusinessState (BusinessStateId);

ALTER TABLE work.WorkItem
ADD CONSTRAINT FK_WorkItem_FinalBusinessStateId FOREIGN KEY (FinalBusinessStateId)
        REFERENCES work.BusinessState (BusinessStateId);

GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.GetDefaultTimeoutMillisecond') AND type = 'FN')
    DROP FUNCTION work.GetDefaultTimeoutMillisecond;
GO

CREATE FUNCTION work.GetDefaultTimeoutMillisecond ()
RETURNS INT
AS
BEGIN
    RETURN (5 * 60 * 1000);
END;
GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.TryGetWork') AND type = 'P')
    DROP PROCEDURE work.TryGetWork;
GO

CREATE PROCEDURE work.TryGetWork
(
    @systemId                   UNIQUEIDENTIFIER,
    @workflowStateIds           dbo.IntList READONLY,
    @maxWorkItems               INT
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE wi
    SET
        wi.SystemStatusId = 3,          -- Timed-out
        wi.LockTimeoutUtc = NULL,
        wi.LockSystemId = NULL
    FROM work.WorkItem wi
    WHERE
        wi.SystemStatusId = 2           -- Processing
    AND wi.LockTimeoutUtc <= GETUTCDATE()
    ;


    UPDATE wi
    SET
        wi.SystemStatusId = CASE
                                WHEN wi.RetryCount < bs.MaxRetryCount THEN 4    -- Rescheduled
                                WHEN bs.MaxRetryCount IS NULL         THEN 4    -- Rescheduled
                                ELSE 5                                          -- Failed
                            END,
        wi.RetryCount     = CASE
                                WHEN wi.RetryCount < bs.MaxRetryCount THEN wi.RetryCount + 1
                                WHEN bs.MaxRetryCount IS NULL         THEN wi.RetryCount + 1
                                ELSE wi.RetryCount
                            END
    FROM work.WorkItem wi
    LEFT JOIN work.BusinessState bs
            ON  bs.BusinessStateId = wi.InitialBusinessStateId
    WHERE
        wi.SystemStatusId = 3       -- Timed-out
    ;


	-- TODO Custom selectors

    DECLARE @workItemIds TABLE
    (
        WorkItemId        BIGINT
    )

	BEGIN TRANSACTION

		INSERT INTO @workItemIds
		(
			WorkItemId
		)
		SELECT TOP (@maxWorkItems)
			wi.WorkItemId
		FROM work.WorkItem wi WITH (XLOCK, HOLDLOCK) -- TODO: review required locks
		JOIN work.BusinessState bs
				ON  bs.BusinessStateId = wi.InitialBusinessStateId
		WHERE
			wi.InitialBusinessStateId IN (SELECT [Value] FROM @workflowStateIds)
		AND bs.CustomSelectFunc IS NULL
		AND wi.SystemStatusId IN (1, 4)
		ORDER BY
			wi.[Priority] DESC,
			wi.WorkItemId ASC
		;

		UPDATE wi
		SET
			wi.SystemStatusId = 2,                -- Processing
			wi.LockSystemId = @systemId,
            wi.LockTimeoutUtc = DATEADD(SECOND, ISNULL(bs.TimeoutMillisecond, work.GetDefaultTimeoutMillisecond()), GETUTCDATE())
		OUTPUT
			inserted.WorkflowId,
			inserted.WorkItemId,
			inserted.[Sequence],
			inserted.InitialBusinessStateId,
            inserted.LockTimeoutUtc,
			inserted.DataId
        FROM work.WorkItem wi
        JOIN work.BusinessState bs
                ON  bs.BusinessStateId = wi.InitialBusinessStateId
		JOIN @workItemIds wiid
				ON  wiid.WorkItemId = wi.WorkItemId
		;

	COMMIT

    SELECT
	    wid.DataId,
		wid.[Key],
		wid.[Value]
    FROM work.WorkItemData wid
    JOIN work.WorkItem wi
	        ON  wi.DataId = wid.DataId
            AND wi.DataVersion >= wid.DataVersion
	JOIN @workItemIds wiid
            ON  wiid.WorkItemId = wi.WorkItemId
    ;
END
GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.TryGetWorkItemLock') AND type = 'P')
    DROP PROCEDURE work.TryGetWorkItemLock;
GO

CREATE PROCEDURE work.TryGetWorkItemLock
(
    @systemId                   UNIQUEIDENTIFIER,
    @workItemId                 BIGINT,

    @result                     BIT         OUTPUT,
    @timeout                    DATETIME    OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION
        
        
        UPDATE wi
        SET
            wi.SystemStatusId   = 2,  -- Processing
            wi.LockSystemId     = @systemId,
            @timeout            = DATEADD(SECOND, ISNULL(bs.TimeoutMillisecond, work.GetDefaultTimeoutMillisecond()), GETUTCDATE()),
            wi.LockTimeoutUtc   = DATEADD(SECOND, ISNULL(bs.TimeoutMillisecond, work.GetDefaultTimeoutMillisecond()), GETUTCDATE())
        FROM work.WorkItem wi
        JOIN work.BusinessState bs
                ON  bs.BusinessStateId = wi.InitialBusinessStateId
        WHERE
            wi.SystemStatusId IN (1, 4)
        AND wi.WorkItemId = @workItemId
		AND wi.LockSystemId IS NULL
    
        
        SELECT
            @result = CONVERT(BIT, CASE WHEN (@@ROWCOUNT = 1) THEN 1 ELSE 0 END)
        ;

        IF (@result <> 1)
        BEGIN
            ROLLBACK;
            RETURN;
        END

    COMMIT
END
GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.TryRefreshWorkItemLock') AND type = 'P')
    DROP PROCEDURE work.TryRefreshWorkItemLock;
GO

CREATE PROCEDURE work.TryRefreshWorkItemLock
(
    @systemId                   UNIQUEIDENTIFIER,
    @workItemId                 BIGINT,

    @result                     BIT         OUTPUT,
    @timeout                    DATETIME    OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION
        
        
        UPDATE wi
        SET
            @timeout            = DATEADD(SECOND, ISNULL(bs.TimeoutMillisecond, work.GetDefaultTimeoutMillisecond()), GETUTCDATE()),
            wi.LockTimeoutUtc   = DATEADD(SECOND, ISNULL(bs.TimeoutMillisecond, work.GetDefaultTimeoutMillisecond()), GETUTCDATE())
        FROM work.WorkItem wi
        JOIN work.BusinessState bs
                ON  bs.BusinessStateId = wi.InitialBusinessStateId
        WHERE
            wi.SystemStatusId = 2  -- TODO: Do we want to restrain to 2 only?
        AND wi.WorkItemId = @workItemId
		AND wi.LockSystemId = @systemId
    
        
        SELECT
            @result = CONVERT(BIT, CASE WHEN (@@ROWCOUNT = 1) THEN 1 ELSE 0 END)
        ;

        IF (@result <> 1)
        BEGIN
            ROLLBACK;
            RETURN;
        END

    COMMIT
END
GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.MarkAsComplete') AND type = 'P')
    DROP PROCEDURE work.MarkAsComplete;
GO

CREATE PROCEDURE work.MarkAsComplete
(
    @systemId                   UNIQUEIDENTIFIER,
    @workItemId                 BIGINT,
    @finalBusinessStateId       INT,
    @startSequenceAt            INT,

    @result                     BIT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION

        -- TODO: Time-out

        UPDATE wi
        SET
            wi.SystemStatusId = 6,         -- Complete
            wi.FinalBusinessStateId = @finalBusinessStateId,
			wi.LockTimeoutUtc = NULL,
			wi.LockSystemId = NULL
        FROM work.WorkItem wi
        WHERE
            wi.SystemStatusId = 2
        AND wi.WorkItemId = @workItemId
		AND wi.LockSystemId = @systemId
        ;

        SELECT
            @result = CONVERT(BIT, CASE WHEN (@@ROWCOUNT = 1) THEN 1 ELSE 0 END)
        ;

        IF (@result <> 1)
        BEGIN
            ROLLBACK;
            RETURN;
        END


        DECLARE @newWorkItemIds TABLE
        (
            WorkItemId      BIGINT
        )
        ;

        INSERT INTO work.WorkItem
        (
            WorkflowId,
            SystemStatusId,
            [Sequence],
            InitialBusinessStateId,
	        DataId,
	        DataVersion
        )
        OUTPUT inserted.WorkItemId
        INTO @newWorkItemIds
        SELECT
            wi.WorkflowId,
            0, -- Created
            wi.[Sequence] + '-' +
                CONVERT(VARCHAR(10), (@startSequenceAt - 1) + ROW_NUMBER() OVER(ORDER BY bst.BusinessStateTransitionId)),
            bst.NextInitialBusinessStateId,
            wi.DataId,
            wi.DataVersion
        FROM work.BusinessStateTransition bst
        CROSS JOIN work.WorkItem wi
        WHERE
            wi.WorkItemId = @workItemId
        AND bst.PrevFinalBusinessStateId = @finalBusinessStateId
        ORDER BY
            bst.BusinessStateTransitionId
        ;

        UPDATE wi
        SET
            wi.SystemStatusId = 1      -- Ready
        FROM work.WorkItem wi
        WHERE
            wi.WorkItemId IN (SELECT WorkItemId FROM @newWorkItemIds)

    COMMIT
END

GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.MarkAsFailed') AND type = 'P')
    DROP PROCEDURE work.MarkAsFailed;
GO

CREATE PROCEDURE work.MarkAsFailed
(
    @systemId                   UNIQUEIDENTIFIER,
    @workItemId                 BIGINT,

    @result                     BIT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRANSACTION
        
        -- TODO: Time-out
        
        UPDATE wi
        SET
            wi.SystemStatusId = 5,         -- Failed
			wi.LockTimeoutUtc = NULL,
			wi.LockSystemId = NULL
        FROM work.WorkItem wi
        WHERE
            wi.SystemStatusId = 2
        AND wi.WorkItemId = @workItemId
		AND wi.LockSystemId = @systemId
        ;


        SELECT
            @result = CONVERT(BIT, CASE WHEN (@@ROWCOUNT = 1) THEN 1 ELSE 0 END)
        ;

    COMMIT
END

GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.GetAllStatic') AND type = 'P')
    DROP PROCEDURE work.GetAllStatic;
GO

CREATE PROCEDURE work.GetAllStatic
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT
        BusinessStateId,
        [Name],
	    TimeoutMillisecond,
        MaxRetryCount,
        [Description]
    FROM work.BusinessState
    ;

END

GO


IF EXISTS(SELECT 1 FROM sys.types WHERE user_type_id = TYPE_ID('work.WorkItemList') AND is_table_type = 1)
    DROP TYPE work.WorkItemList;
GO

CREATE TYPE work.WorkItemList AS TABLE
(
    WorkflowId                  BIGINT,
    Sequence                    VARCHAR(8000),
    InitialBusinessStateId      INT,
    DataGroupId                 INT
);
GO


IF EXISTS(SELECT 1 FROM sys.types WHERE user_type_id = TYPE_ID('work.DataList') AND is_table_type = 1)
    DROP TYPE work.DataList;
GO

CREATE TYPE work.DataList AS TABLE
(
    DataGroupId                 INT,
    [Key]                       VARCHAR(50),
    Value                       VARCHAR(8000),

     PRIMARY KEY CLUSTERED (DataGroupId, [Key])
);
GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.CreateWorkItems') AND type = 'P')
    DROP PROCEDURE work.CreateWorkItems;
GO

CREATE PROCEDURE work.CreateWorkItems
(
    @workItems                  work.WorkItemList READONLY,
    @data                       work.DataList READONLY
)
AS
BEGIN
    SET NOCOUNT ON;
    

    DECLARE @dataIds TABLE
    (
        DataId          BIGINT NOT NULL
    )
    ;

    INSERT INTO work.WorkItemData
    (
        DataId,
        DataVersion,
        [Key],
        Value
    )
    OUTPUT inserted.DataId
    INTO @dataIds
    SELECT
        maxId.MaxDataId + DENSE_RANK() OVER(ORDER BY d.DataGroupId),
        1,
        d.[Key],
        d.Value
    FROM @data d
    CROSS JOIN (SELECT ISNULL(MAX(DataId), 0) AS MaxDataId FROM work.WorkItemData) maxId
    ;

    DECLARE @maxDataId BIGINT = (SELECT MIN(d.DataId) - 1 FROM @dataIds d)
    ;


    DECLARE @newWorkItemIds TABLE
    (
        WorkItemId      BIGINT
    )
    ;

    INSERT INTO work.WorkItem
    (
        WorkflowId,
        SystemStatusId,
        [Sequence],
        InitialBusinessStateId,
	    DataId,
	    DataVersion
    )
    OUTPUT inserted.WorkItemId
    INTO @newWorkItemIds
    SELECT
        wi.WorkflowId,
        0,              -- CREATED
        wi.[Sequence],
        wi.InitialBusinessStateId,
        @maxDataId + DENSE_RANK() OVER(ORDER BY d.DataGroupId),
	    1
    FROM @workItems wi
    LEFT JOIN (SELECT DISTINCT DataGroupId FROM @data) d
            ON  d.DataGroupId = wi.DataGroupId
    ;

    UPDATE wi
    SET
        SystemStatusId = 1 -- READY
    FROM work.WorkItem wi
    JOIN @newWorkItemIds ids
            ON  ids.WorkItemId = wi.WorkItemId
    ;
END

GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.InitiateWorkflow') AND type = 'P')
    DROP PROCEDURE work.InitiateWorkflow;
GO

CREATE PROCEDURE work.InitiateWorkflow
(
    @externalId                 VARCHAR(8000) = NULL,
    @description                VARCHAR(MAX) = NULL,
    @initialBusinessStateId     INT,

    @data                       dbo.KeyValueList READONLY
)
AS
BEGIN
    SET NOCOUNT ON;


    DECLARE @dataId BIGINT = NULL;
    DECLARE @dataVersion BIGINT = NULL;

    IF (NOT EXISTS (SELECT 1 FROM @data))
    BEGIN
        SET @dataId = NULL;
        SET @dataVersion = NULL;
    END
    ELSE
    BEGIN
        DECLARE @dataIds TABLE
        (
            DataId          BIGINT NOT NULL
        )
        ;

        INSERT INTO work.WorkItemData
        (
            DataId,
            DataVersion,
            [Key],
            Value
        )
        OUTPUT
            inserted.DataId
        INTO @dataIds
        SELECT
            maxId.MaxDataId + 1,
            1,
            d.[Key],
            d.Value
        FROM @data d
        CROSS JOIN (SELECT ISNULL(MAX(DataId), 0) AS MaxDataId FROM work.WorkItemData) maxId
        ;

        SELECT
            @dataId = d.DataId,
            @dataVersion = 1
        FROM @dataIds d
    END


    DECLARE @workflowIds TABLE
    (
        WorkflowId      BIGINT NOT NULL
    )
    ;

    INSERT INTO work.Workflow
    (
        ExternalId,
        [Description]
    )
    OUTPUT inserted.WorkflowId
    INTO @workflowIds
    VALUES
    (
        @externalId,
        @description
    )
    ;


    DECLARE @newWorkItemIds TABLE
    (
        WorkItemId      BIGINT NOT NULL
    )
    ;

    INSERT INTO work.WorkItem
    (
        WorkflowId,
        SystemStatusId,
        [Sequence],
        InitialBusinessStateId,
	    DataId,
	    DataVersion           
    )
    OUTPUT inserted.WorkItemId
    INTO @newWorkItemIds
    VALUES
    (
        (SELECT WorkflowId FROM @workflowIds),
        0, -- CREATED
        '1',
        @initialBusinessStateId,
	    @dataId,
	    @dataVersion
    )
    ;

    UPDATE wi
    SET
        wi.SystemStatusId = 1 -- READY
    FROM work.WorkItem wi
    JOIN @newWorkItemIds ids
            ON  ids.WorkItemId = wi.WorkItemId
    ;


    IF (EXISTS (SELECT 1 FROM work.BusinessStateTransition bst WHERE bst.PrevFinalBusinessStateId = @initialBusinessStateId))
    BEGIN
        DECLARE @systemId                   UNIQUEIDENTIFIER = NEWID();
        DECLARE @workItemId                 BIGINT = (SELECT MAX(WorkItemId) FROM @newWorkItemIds);
        DECLARE @finalBusinessStateId       INT = @initialBusinessStateId
        DECLARE @startSequenceAt            INT = 1
        DECLARE @result                     BIT;
        DECLARE @timeout                    DATETIME;

        EXEC work.TryGetWorkItemLock
            @systemId,
            @workItemId,
            @result  OUTPUT,
            @timeout OUTPUT
        ;

        IF (@result = 1)
        BEGIN
            EXEC work.MarkAsComplete
                @systemId,
                @workItemId,
                @finalBusinessStateId,
                @startSequenceAt,
                @result OUTPUT
            ;
        END
    END
END

GO


IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('work.ReadableWorkItems') AND type = 'V')
    DROP VIEW work.ReadableWorkItems;
GO

CREATE VIEW work.ReadableWorkItems AS
SELECT
    wi.WorkItemId,
    wi.WorkflowId,
    wi.[Priority],
    wi.SystemStatusId,
    ss.Name                 AS SystemStateName,
    wi.[Sequence],
    wi.InitialBusinessStateId,
    bs_initial.Name         AS InitialBusinessStateName,
    wi.FinalBusinessStateId,
    bs_final.Name           AS FinalBusinessStateName,
    wi.LockTimeoutUtc,
    wi.LockSystemId,
    wi.RetryCount,
	wi.DataId,
	wi.DataVersion,
    (
        SELECT DISTINCT
            LEFT(r.KeyValuePair , LEN(r.KeyValuePair)-1) AS KeyValuePairs
        FROM work.WorkItemData e
        CROSS APPLY
        (
            SELECT r.[Key] + '=' + r.Value + ', '
            FROM work.WorkItemData r
            WHERE
                r.DataId = e.DataId
            AND r.DataVersion <= wi.DataVersion
            ORDER BY r.[Key]
            FOR XML PATH('')
        ) r (KeyValuePair)
        WHERE
            e.DataId = wi.DataId
    ) AS Data
FROM work.WorkItem wi
JOIN work.SystemStatus ss
        ON  ss.SystemStatusId = wi.SystemStatusId
JOIN work.BusinessState bs_initial
        ON  bs_initial.BusinessStateId = wi.InitialBusinessStateId
LEFT JOIN work.BusinessState bs_final
        ON  bs_final.BusinessStateId = wi.FinalBusinessStateId
;
