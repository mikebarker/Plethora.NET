DELETE FROM work.BusinessStateTransition;
DELETE FROM work.BusinessState;

INSERT INTO work.BusinessState
([Name], TimeoutMillisecond, MaxRetryCount, CustomSelectFunc, [Description])
VALUES
('Example.Initial',           5000, 5, NULL, NULL),
('Example.Stage1',            5000, 5, NULL, NULL),
('Example.Stage1Complete',    5000, 5, NULL, NULL),
('Example.Stage2',            5000, 5, NULL, NULL),
('Example.Stage2Complete',    5000, 5, NULL, NULL),
('Example.Stage3-1',          5000, 5, NULL, NULL),
('Example.Stage3-1Complete',  5000, 5, NULL, NULL),
('Example.Stage3-2',          5000, 5, NULL, NULL),
('Example.Stage3-2Complete',  5000, 5, NULL, NULL)
;

INSERT INTO work.BusinessStateTransition
(PrevFinalBusinessStateId, NextInitialBusinessStateId, [Description])
SELECT
    (SELECT BusinessStateId FROM work.BusinessState WHERE Name = 'Example.Initial'),
    (SELECT BusinessStateId FROM work.BusinessState WHERE Name = 'Example.Stage1'),
    'Initial state'
;

INSERT INTO work.BusinessStateTransition
(PrevFinalBusinessStateId, NextInitialBusinessStateId, [Description])
SELECT
    (SELECT BusinessStateId FROM work.BusinessState WHERE Name = 'Example.Stage1Complete'),
    (SELECT BusinessStateId FROM work.BusinessState WHERE Name = 'Example.Stage2'),
    NULL
;

INSERT INTO work.BusinessStateTransition
(PrevFinalBusinessStateId, NextInitialBusinessStateId, [Description])
SELECT
    (SELECT BusinessStateId FROM work.BusinessState WHERE Name = 'Example.Stage2Complete'),
    (SELECT BusinessStateId FROM work.BusinessState WHERE Name = 'Example.Stage3-1'),
    NULL
;

INSERT INTO work.BusinessStateTransition
(PrevFinalBusinessStateId, NextInitialBusinessStateId, [Description])
SELECT
    (SELECT BusinessStateId FROM work.BusinessState WHERE Name = 'Example.Stage2Complete'),
    (SELECT BusinessStateId FROM work.BusinessState WHERE Name = 'Example.Stage3-2'),
    NULL
;

SELECT
    bst.BusinessStateTransitionId,
    bst.[Description],
    bst.PrevFinalBusinessStateId,
    bs_prev.Name,
    bst.NextInitialBusinessStateId,
    bs_next.Name
FROM work.BusinessStateTransition bst
JOIN work.BusinessState bs_prev
        ON bs_prev.BusinessStateId = bst.PrevFinalBusinessStateId
JOIN work.BusinessState bs_next
        ON bs_next.BusinessStateId = bst.NextInitialBusinessStateId
;
