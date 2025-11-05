


/*********************************

[P_getUserIDforReports]

**********************************/

CREATE proc P_getUserIDforReports

As

SELECT UserID from t_companyUser
where Login = 'Support1'


