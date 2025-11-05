

/*
Post-Deployment Script Template							
*/



UPDATE T_SAPISnapshotRequestField
SET BBGMnemonic = 'EQY_PRIM_EXCH_SHRT'
WHERE NirvanaFields = 'Exchange';
