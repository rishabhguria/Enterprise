CREATE TABLE T_ThirdPartyFileStatus
(
    id INT PRIMARY KEY Identity(1,1) NOT NULL,
    BatchRunDate DATETIME NOT NULL,
    ThirdPartyBatchId INT NOT NULL
);

