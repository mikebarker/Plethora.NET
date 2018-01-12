DECLARE @externalId                 VARCHAR(8000) = null;
DECLARE @description                VARCHAR(MAX) = 'Testing the StartWorkflow command';
DECLARE @initialBusinessStateId     INT = (SELECT BusinessStateId FROM work.BusinessState WHERE Name = 'Example.Initial');
DECLARE @data				        dbo.KeyValueList;

EXEC work.StartWorkflow @externalId, @description, @initialBusinessStateId, @data
;

SELECT * FROM work.ReadableWorkItems;
