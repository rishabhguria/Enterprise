<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select="'true'"/>
					</RowHeader>
					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<!--Blue Charm-->
					<Client_Code>
						<xsl:value-of select="'BLU'"/>
					</Client_Code>

					<Fund_Code>
						<xsl:value-of select="AccountMappedName"/>
					</Fund_Code>
					<External_Reference>
						<xsl:value-of select="PBUniqueID"/>
					</External_Reference>

					<xsl:choose>
						<xsl:when test="Side ='Buy' or Side='Buy to Open'">
							<Transaction_Type>
								<xsl:value-of select="'B'"/>
							</Transaction_Type>
						</xsl:when>
						<xsl:when test="Side ='Buy to Close'">
							<Transaction_Type>
								<xsl:value-of select="'BC'"/>
							</Transaction_Type>
						</xsl:when>
						<xsl:when test="Side='Sell short'or Side='Sell to Open'">
							<Transaction_Type>
								<xsl:value-of select="'SS'"/>
							</Transaction_Type>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<Transaction_Type>
								<xsl:value-of select="'S'"/>
							</Transaction_Type>
						</xsl:when>
						<xsl:otherwise>
							<Transaction_Type>
								<xsl:value-of select="''"/>
							</Transaction_Type>
						</xsl:otherwise>
					</xsl:choose>


					<Free_Of_Patment_Ind>
						<xsl:value-of select="''"/>
					</Free_Of_Patment_Ind>
					<Position_Type>
						<xsl:value-of select="''"/>
					</Position_Type>
					<Msg_Type>
						<xsl:value-of select="''"/>
					</Msg_Type>
					<External_Block_Ref>
						<xsl:value-of select="''"/>
					</External_Block_Ref>
					<Product_Type>
						<xsl:value-of select="''"/>
					</Product_Type>
					<ISIN>
						<xsl:value-of select="ISIN"/>
					</ISIN>

					<SEDOL>
						<xsl:value-of select="SEDOL"/>
					</SEDOL>
					<Asset_Name>
						<xsl:value-of select="''"/>
					</Asset_Name>
					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>
					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>
					<Price_Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Price_Currency>
					<Price_Factor>
						<xsl:value-of select="''"/>
					</Price_Factor>



					<Trade_Date>
						<xsl:value-of select="concat(substring(TradeDate,7,4),substring(TradeDate,1,2),substring(TradeDate,4,2))"/>
					</Trade_Date>
					<Execution_DateTime>
						<xsl:value-of select="''"/>
					</Execution_DateTime>
					<Settlement_Date>
						<xsl:value-of select="concat(substring(SettlementDate,7,4),substring(SettlementDate,1,2),substring(SettlementDate,4,2))"/>
					</Settlement_Date>
					<External_Exec_Broker>
						<xsl:value-of select="CounterParty"/>
					</External_Exec_Broker>
					<Gross_Amount>
						<xsl:value-of select="GrossAmount"/>
					</Gross_Amount>
					<Tax>
						<xsl:value-of select="''"/>
					</Tax>
					<Broker_Commission>
						<xsl:value-of select="''"/>
					</Broker_Commission>
					<Expenses1>
						<xsl:value-of select="''"/>
					</Expenses1>
					<Expenses2>
						<xsl:value-of select="''"/>
					</Expenses2>
					<Accrued_Interest>
						<xsl:value-of select="''"/>
					</Accrued_Interest>
					<Days_Accrued>
						<xsl:value-of select="''"/>
					</Days_Accrued>

					<Net_Consideration>
						<xsl:value-of select="NetAmount"/>
					</Net_Consideration>

					<Settlement_Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Settlement_Currency>

					<xsl:choose>
						<xsl:when test="TaxLotState='Allocated' or TaxLotState='Amemded' or TaxLotState='Sent'">
							<External_Msg_Function>
								<xsl:value-of select="'NEWM'"/>
							</External_Msg_Function>
						</xsl:when>
						<xsl:when test="TaxLotState='Deleted'">
							<External_Msg_Function>
								<xsl:value-of select="'CANC'"/>
							</External_Msg_Function>
						</xsl:when>
						<xsl:otherwise>
							<External_Msg_Function>
								<xsl:value-of select="''"/>
							</External_Msg_Function>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="TaxLotState='Deleted'">
							<External_Related_Ref>
								<xsl:value-of select="PBUniqueID"/>
							</External_Related_Ref>
						</xsl:when>
						<xsl:otherwise>
							<External_Related_Ref>
								<xsl:value-of select="''"/>
							</External_Related_Ref>
						</xsl:otherwise>
					</xsl:choose>


					<Bargain_Condition>
						<xsl:value-of select="''"/>
					</Bargain_Condition>
					<Agency_Priciple_Ind>
						<xsl:value-of select="''"/>
					</Agency_Priciple_Ind>
					<Maturity_Date>
						<xsl:value-of select="''"/>
					</Maturity_Date>
					<Yield>
						<xsl:value-of select="''"/>
					</Yield>
					<External_Clrg_Broker>
						<xsl:value-of select="''"/>
					</External_Clrg_Broker>

					<Place_of_Settlement>
						<xsl:value-of select="''"/>
					</Place_of_Settlement>

					<FX_Instruction>
						<xsl:value-of select="''"/>
					</FX_Instruction>
					<Sec_Account_at_Counterparty>
						<xsl:value-of select="''"/>
					</Sec_Account_at_Counterparty>
					<Sec_Beneficiary_Address>
						<xsl:value-of select="''"/>
					</Sec_Beneficiary_Address>
					<Beneficiary_Sec_Account_No>
						<xsl:value-of select="''"/>
					</Beneficiary_Sec_Account_No>
					<Cash_Beneficiary_Address>
						<xsl:value-of select="''"/>
					</Cash_Beneficiary_Address>
					<Beneficiary_Cash_Account_Type>
						<xsl:value-of select="''"/>
					</Beneficiary_Cash_Account_Type>
					<Beneficiary_Cash_Account_Number>
						<xsl:value-of select="''"/>
					</Beneficiary_Cash_Account_Number>
					<Stamp_Declaration>
						<xsl:value-of select="''"/>
					</Stamp_Declaration>
					<Nationality_Declaration>
						<xsl:value-of select="''"/>
					</Nationality_Declaration>
					<Free_Format_Special_Instruction>
						<xsl:value-of select="''"/>
					</Free_Format_Special_Instruction>
					<Strategy_Free_Format>
						<xsl:value-of select="''"/>
					</Strategy_Free_Format>

					<End_File>
						<xsl:value-of select="'END'"/>
					</End_File>



					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>