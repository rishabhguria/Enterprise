using Microsoft.IdentityModel.Tokens;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Models.RequestDto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prana.ServiceGateway.UnitTest.Commons
{
    public class ServiceGatewayTestCommons
    {

        #region Common Data
        public static readonly string orderSide = "{\"Equity\":[{\"SideID\":1,\"Name\":\"Buy\",\"TagValue\":\"1\",\"AUECID\":-2147483648,\"CVAUECID\":-2147483648},{\"SideID\":2,\"Name\":\"Sell\",\"TagValue\":\"2\",\"AUECID\":-2147483648,\"CVAUECID\":-2147483648},{\"SideID\":5,\"Name\":\"Sell short\",\"TagValue\":\"5\",\"AUECID\":-2147483648,\"CVAUECID\":-2147483648},{\"SideID\":10,\"Name\":\"Buy to Close\",\"TagValue\":\"B\",\"AUECID\":-2147483648,\"CVAUECID\":-2147483648}]}";

        public static readonly AuthenticationRequestDto userDetailsDto = new AuthenticationRequestDto
        {
            UserName = "testUserName",
            Password = new List<int> { 117, 113, 107 }
        }; 

        public static readonly string symbol = "{\"TickerSymbol\":\"MSFT\"}";

        public static readonly string blotterData = "{\"OrderTab\":[{\"OrderId\":\"2021111610291846\",\"StagedOrderID\":\"\",\"ParentClOrderID\":\"2021111610291846\",\"ClOrderID\":\"2021111610291850\",\"TransactionTime\":\"11/16/2021 12:02:15 AM\",\"Symbol\":\"MSFT\",\"Side\":\"Buy\",\"OrderSideTagValue\":\"1\",\"OrderType\":\"Market\",\"OrderTypeTagValue\":\"1\",\"Status\":\"Replaced\",\"OrderStatusTagValue\":\"5\",\"Quantity\":2000.0,\"BrokerID\":1,\"Broker\":\"MS SWAP\",\"WorkingQuantity\":800.0,\"ExecutedQuantity\":1200.0,\"AvgFillPrice\":337.42,\"AccountID\":-2147483648,\"Account\":\"-\",\"AllocationSchemeName\":\"-\",\"Limit\":0.0,\"CumQty\":1200.0,\"PercentExecuted\":60.0,\"VenueID\":1,\"Venue\":\"Drops\",\"AlgoStrategyID\":\"\",\"Algo\":\"\",\"TIF\":\"0\",\"Trader\":\"Octahedron\",\"OrderCollection\":[{\"OrderId\":\"2021111610291847\",\"StagedOrderID\":\"2021111610291846\",\"ParentClOrderID\":\"2021111610291847\",\"ClOrderID\":\"2021111610291851\",\"TransactionTime\":\"11/16/2021 12:02:15 AM\",\"Symbol\":\"MSFT\",\"Side\":\"Buy\",\"OrderSideTagValue\":\"1\",\"OrderType\":\"Market\",\"OrderTypeTagValue\":\"1\",\"Status\":\"Replaced\",\"OrderStatusTagValue\":\"5\",\"Quantity\":2000.0,\"BrokerID\":1,\"Broker\":\"MS SWAP\",\"WorkingQuantity\":800.0,\"ExecutedQuantity\":1200.0,\"AvgFillPrice\":337.42,\"AccountID\":-2147483648,\"Account\":\"-\",\"AllocationSchemeName\":\"-\",\"Limit\":0.0,\"CumQty\":1200.0,\"PercentExecuted\":60.0,\"VenueID\":1,\"Venue\":\"Drops\",\"AlgoStrategyID\":\"\",\"Algo\":\"\",\"TIF\":\"0\",\"Trader\":\"Octahedron\",\"OrderCollection\":null}]},{\"OrderId\":\"2021111610291852\",\"StagedOrderID\":\"\",\"ParentClOrderID\":\"2021111610291852\",\"ClOrderID\":\"2021111610291854\",\"TransactionTime\":\"11/16/2021 12:08:40 AM\",\"Symbol\":\"IBM\",\"Side\":\"Buy\",\"OrderSideTagValue\":\"1\",\"OrderType\":\"Market\",\"OrderTypeTagValue\":\"1\",\"Status\":\"PartiallyFilled\",\"OrderStatusTagValue\":\"1\",\"Quantity\":5000.0,\"BrokerID\":1,\"Broker\":\"MS SWAP\",\"WorkingQuantity\":500.0,\"ExecutedQuantity\":2500.0,\"AvgFillPrice\":12.0,\"AccountID\":-2147483648,\"Account\":\"-\",\"AllocationSchemeName\":\"-\",\"Limit\":0.0,\"CumQty\":2500.0,\"PercentExecuted\":50.0,\"VenueID\":1,\"Venue\":\"Drops\",\"AlgoStrategyID\":\"\",\"Algo\":\"\",\"TIF\":\"0\",\"Trader\":\"Octahedron\",\"OrderCollection\":[{\"OrderId\":\"2021111610291853\",\"StagedOrderID\":\"2021111610291852\",\"ParentClOrderID\":\"2021111610291853\",\"ClOrderID\":\"2021111610291853\",\"TransactionTime\":\"11/16/2021 12:08:40 AM\",\"Symbol\":\"IBM\",\"Side\":\"Buy\",\"OrderSideTagValue\":\"1\",\"OrderType\":\"Market\",\"OrderTypeTagValue\":\"1\",\"Status\":\"PartiallyFilled\",\"OrderStatusTagValue\":\"1\",\"Quantity\":3000.0,\"BrokerID\":1,\"Broker\":\"MS SWAP\",\"WorkingQuantity\":500.0,\"ExecutedQuantity\":2500.0,\"AvgFillPrice\":12.0,\"AccountID\":-2147483648,\"Account\":\"-\",\"AllocationSchemeName\":\"-\",\"Limit\":0.0,\"CumQty\":2500.0,\"PercentExecuted\":83.333333333333343,\"VenueID\":1,\"Venue\":\"Drops\",\"AlgoStrategyID\":\"\",\"Algo\":\"N.A.\",\"TIF\":\"0\",\"Trader\":\"Octahedron\",\"OrderCollection\":null}]},{\"OrderId\":\"2022072313152301\",\"StagedOrderID\":\"\",\"ParentClOrderID\":\"2022072313152301\",\"ClOrderID\":\"2022072313152301\",\"TransactionTime\":\"7/23/2022 3:51:30 AM\",\"Symbol\":\"IBM\",\"Side\":\"Buy\",\"OrderSideTagValue\":\"1\",\"OrderType\":\"Market\",\"OrderTypeTagValue\":\"1\",\"Status\":\"New\",\"OrderStatusTagValue\":\"0\",\"Quantity\":5.0,\"BrokerID\":1,\"Broker\":\"MS SWAP\",\"WorkingQuantity\":0.0,\"ExecutedQuantity\":0.0,\"AvgFillPrice\":0.0,\"AccountID\":-2147483648,\"Account\":\"-\",\"AllocationSchemeName\":\"-\",\"Limit\":0.0,\"CumQty\":0.0,\"PercentExecuted\":0.0,\"VenueID\":1,\"Venue\":\"Drops\",\"AlgoStrategyID\":\"\",\"Algo\":\"N.A.\",\"TIF\":\"0\",\"Trader\":\"Octahedron\",\"OrderCollection\":[]},{\"OrderId\":\"2022072313152303\",\"StagedOrderID\":\"\",\"ParentClOrderID\":\"2022072313152303\",\"ClOrderID\":\"2022072313152303\",\"TransactionTime\":\"7/23/2022 4:41:50 AM\",\"Symbol\":\"AAPL\",\"Side\":\"Buy\",\"OrderSideTagValue\":\"1\",\"OrderType\":\"Market\",\"OrderTypeTagValue\":\"1\",\"Status\":\"New\",\"OrderStatusTagValue\":\"0\",\"Quantity\":1.0,\"BrokerID\":1,\"Broker\":\"MS SWAP\",\"WorkingQuantity\":0.0,\"ExecutedQuantity\":0.0,\"AvgFillPrice\":0.0,\"AccountID\":-2147483648,\"Account\":\"-\",\"AllocationSchemeName\":\"-\",\"Limit\":0.0,\"CumQty\":0.0,\"PercentExecuted\":0.0,\"VenueID\":1,\"Venue\":\"Drops\",\"AlgoStrategyID\":\"\",\"Algo\":\"N.A.\",\"TIF\":\"0\",\"Trader\":\"Octahedron\",\"OrderCollection\":[]},{\"OrderId\":\"2022072313152304\",\"StagedOrderID\":\"\",\"ParentClOrderID\":\"2022072313152304\",\"ClOrderID\":\"2022072313152304\",\"TransactionTime\":\"7/23/2022 4:42:27 AM\",\"Symbol\":\"AAPL\",\"Side\":\"Buy\",\"OrderSideTagValue\":\"1\",\"OrderType\":\"Market\",\"OrderTypeTagValue\":\"1\",\"Status\":\"New\",\"OrderStatusTagValue\":\"0\",\"Quantity\":10.0,\"BrokerID\":1,\"Broker\":\"MS SWAP\",\"WorkingQuantity\":0.0,\"ExecutedQuantity\":0.0,\"AvgFillPrice\":0.0,\"AccountID\":-2147483648,\"Account\":\"-\",\"AllocationSchemeName\":\"-\",\"Limit\":0.0,\"CumQty\":0.0,\"PercentExecuted\":0.0,\"VenueID\":1,\"Venue\":\"Drops\",\"AlgoStrategyID\":\"\",\"Algo\":\"N.A.\",\"TIF\":\"0\",\"Trader\":\"Octahedron\",\"OrderCollection\":[]},{\"OrderId\":\"2022072313152306\",\"StagedOrderID\":\"\",\"ParentClOrderID\":\"2022072313152306\",\"ClOrderID\":\"2022072313152306\",\"TransactionTime\":\"7/23/2022 4:43:44 AM\",\"Symbol\":\"A\",\"Side\":\"Buy\",\"OrderSideTagValue\":\"1\",\"OrderType\":\"Market\",\"OrderTypeTagValue\":\"1\",\"Status\":\"New\",\"OrderStatusTagValue\":\"0\",\"Quantity\":1.0,\"BrokerID\":1,\"Broker\":\"MS SWAP\",\"WorkingQuantity\":0.0,\"ExecutedQuantity\":0.0,\"AvgFillPrice\":0.0,\"AccountID\":-2147483648,\"Account\":\"-\",\"AllocationSchemeName\":\"-\",\"Limit\":0.0,\"CumQty\":0.0,\"PercentExecuted\":0.0,\"VenueID\":1,\"Venue\":\"Drops\",\"AlgoStrategyID\":\"\",\"Algo\":\"N.A.\",\"TIF\":\"0\",\"Trader\":\"Octahedron\",\"OrderCollection\":[]},{\"OrderId\":\"2022072313152307\",\"StagedOrderID\":\"\",\"ParentClOrderID\":\"2022072313152307\",\"ClOrderID\":\"2022072313152307\",\"TransactionTime\":\"7/23/2022 5:14:32 AM\",\"Symbol\":\"IBM\",\"Side\":\"Buy\",\"OrderSideTagValue\":\"1\",\"OrderType\":\"Market\",\"OrderTypeTagValue\":\"1\",\"Status\":\"New\",\"OrderStatusTagValue\":\"0\",\"Quantity\":10.0,\"BrokerID\":1,\"Broker\":\"MS SWAP\",\"WorkingQuantity\":0.0,\"ExecutedQuantity\":0.0,\"AvgFillPrice\":0.0,\"AccountID\":-2147483648,\"Account\":\"-\",\"AllocationSchemeName\":\"-\",\"Limit\":0.0,\"CumQty\":0.0,\"PercentExecuted\":0.0,\"VenueID\":1,\"Venue\":\"Drops\",\"AlgoStrategyID\":\"\",\"Algo\":\"N.A.\",\"TIF\":\"0\",\"Trader\":\"Octahedron\",\"OrderCollection\":[]}],\"WorkingTab\":[]}";
        #endregion

        #region Common functions
        public static string GetJWTToken(int companyUserId, bool isAdminUser = false, bool isSupportUser = true)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("#$nirvana@SamsaraWebApplication$#@"));
                var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                var claims = new[]
                   {
                    new Claim(ApiContants.ADMIN, isAdminUser.ToString()),
                    new Claim(ApiContants.SUPPORT, isSupportUser.ToString()),
                    new Claim(ApiContants.COMPANY_USER_ID, companyUserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                };

                int timeout = 120;
                string sessionTimeOutStr = "1440";
                var isParse = Int32.TryParse(sessionTimeOutStr, out timeout);
                timeout = isParse ? timeout : 120;

                var tokenDescriptor = new JwtSecurityToken
                (
                    issuer: "nirvanaSolutions",
                    audience: "nirvanasolutions.com",
                    expires: DateTime.UtcNow.AddMinutes(timeout),
                    claims: claims,
                    signingCredentials: signinCredentials
                );
                return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public static ClaimsIdentity GetClaimsIdentity(int companyUserId, bool isAdminUser = false, bool isSupportUser = true)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ApiContants.ADMIN, isAdminUser.ToString()),
                    new Claim(ApiContants.SUPPORT, isSupportUser.ToString()),
                    new Claim(ApiContants.COMPANY_USER_ID, companyUserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

                return claimsIdentity;
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion
    }
}
