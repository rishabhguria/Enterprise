<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>
				
				<FileHeader>
					<xsl:value-of select="'true'"/>
				</FileHeader>

				

				<PRODUCTID>
					<xsl:value-of select="'PRODUCT ID'"/>
				</PRODUCTID>

				<TRADEID>
					<xsl:value-of select="'TRADE ID'"/>
				</TRADEID>

				<TRANSACTION>
					<xsl:value-of select="'TRANSACTION'"/>
				</TRANSACTION>

				<TRADESTATUS>
					<xsl:value-of select="'TRADE STATUS'"/>
				</TRADESTATUS>


				<TRADEDATE>
					<xsl:value-of select="'TRADE DATE'"/>
				</TRADEDATE>

				<SETTLEDATE>
					<xsl:value-of select="'SETTLE DATE'"/>
				</SETTLEDATE>

				<BROKER>
					<xsl:value-of select="'BROKER'"/>
				</BROKER>

				<CUSTODIAN>
					<xsl:value-of select="'CUSTODIAN'"/>
				</CUSTODIAN>


				<ACCOUNTCODE>
					<xsl:value-of select="'ACCOUNT CODE'"/>
				</ACCOUNTCODE>

				<QUANTITY>
					<xsl:value-of select="'QUANTITY'"/>
				</QUANTITY>

				<AVERAGEPRICE>
					<xsl:value-of select="'AVERAGE PRICE'"/>
				</AVERAGEPRICE>

				<TOTALSECFEES>
					<xsl:value-of select="'TOTAL SEC FEES'"/>
				</TOTALSECFEES>

				<TOTALNONSECFEES>
					<xsl:value-of select="'TOTAL NON-SEC FEES'"/>
				</TOTALNONSECFEES>

				<TOTALCOMMISSION>
					<xsl:value-of select="'TOTAL COMMISSION'"/>
				</TOTALCOMMISSION>

				<ACCRUEDINTEREST>
					<xsl:value-of select="'ACCRUED INTEREST'"/>
				</ACCRUEDINTEREST>

				<NETAMOUNT>
					<xsl:value-of select="'NET AMOUNT'"/>
				</NETAMOUNT>

				<SETTLEMENTCURRENCY>
					<xsl:value-of select="'SETTLEMENT CURRENCY'"/>
				</SETTLEMENTCURRENCY>

				<TAXLOTID>
					<xsl:value-of select="'TAX LOT ID'"/>
				</TAXLOTID>

				<VERSUSDATE>
					<xsl:value-of select="'VERSUS DATE'"/>
				</VERSUSDATE>

				<STRATEGY1>
					<xsl:value-of select="'STRATEGY1'"/>
				</STRATEGY1>


				<STRATEGY2>
					<xsl:value-of select="'STRATEGY2'"/>
				</STRATEGY2>

				<STRATEGY3>
					<xsl:value-of select="'STRATEGY3'"/>
				</STRATEGY3>

				<STRATEGY4>
					<xsl:value-of select="'STRATEGY4'"/>
				</STRATEGY4>

				<STRATEGY5>
					<xsl:value-of select="'STRATEGY5'"/>
				</STRATEGY5>

				<SECURITYDESCRIPTION>
					<xsl:value-of select="'SECURITY DESCRIPTION'"/>
				</SECURITYDESCRIPTION>

				<SECURITIYCURRENCY>
					<xsl:value-of select="'SECURITIY CURRENCY'"/>
				</SECURITIYCURRENCY>

				<SECURITYTYPE>
					<xsl:value-of select="'SECURITY TYPE'"/>
				</SECURITYTYPE>

				<MULTIPLIER>
					<xsl:value-of select="'MULTIPLIER'"/>
				</MULTIPLIER>

				<OTCINDICATOR>
					<xsl:value-of select="'OTC INDICATOR'"/>
				</OTCINDICATOR>

				<SEDOL>
					<xsl:value-of select="'SEDOL'"/>
				</SEDOL>

				<ISIN>
					<xsl:value-of select="'ISIN'"/>
				</ISIN>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<SECTOR>
					<xsl:value-of select="'SECTOR'"/>
				</SECTOR>


				<INDUSTRY>
					<xsl:value-of select="'INDUSTRY'"/>
				</INDUSTRY>

				<EXCHANGE>
					<xsl:value-of select="'EXCHANGE'"/>
				</EXCHANGE>


				<OCCCODE>
					<xsl:value-of select="'OCC CODE'"/>
				</OCCCODE>

				<UNDERLYINGSECURITY>
					<xsl:value-of select="'UNDERLYING SECURITY'"/>
				</UNDERLYINGSECURITY>

				<STRIKEPRICE>
					<xsl:value-of select="'STRIKE PRICE'"/>
				</STRIKEPRICE>

				<PUTCALL>
					<xsl:value-of select="'PUT/CALL'"/>
				</PUTCALL>

				<EXERCISESTYLE>
					<xsl:value-of select="'EXERCISE STYLE'"/>
				</EXERCISESTYLE>


				<EXPIRATIONDATE>
					<xsl:value-of select="'EXPIRATION DATE'"/>
				</EXPIRATIONDATE>

				<MATURITYTYPE>
					<xsl:value-of select="'MATURITY TYPE'"/>
				</MATURITYTYPE>

				<MATURITY>
					<xsl:value-of select="'MATURITY'"/>
				</MATURITY>

				<PARAMOUNT>
					<xsl:value-of select="'PAR AMOUNT'"/>
				</PARAMOUNT>


				<ISSUEDATE>
					<xsl:value-of select="'ISSUE DATE'"/>
				</ISSUEDATE>

				<EFFECTIVEDATE>
					<xsl:value-of select="'EFFECTIVE DATE'"/>
				</EFFECTIVEDATE>


				<FIRSTPAYMENTDATE>
					<xsl:value-of select="'FIRST PAYMENT DATE'"/>
				</FIRSTPAYMENTDATE>

				<COUPONRATE>
					<xsl:value-of select="'COUPON RATE'"/>
				</COUPONRATE>

				<COUPONFREQUENCY>
					<xsl:value-of select="'COUPON FREQUENCY'"/>
				</COUPONFREQUENCY>

				<ACCRUALMETHOD>
					<xsl:value-of select="'ACCRUAL METHOD'"/>
				</ACCRUALMETHOD>

				<DEBTTYPE>
					<xsl:value-of select="'DEBT TYPE'"/>
				</DEBTTYPE>


				<BUSINESSDAYCONVENTION>
					<xsl:value-of select="'BUSINESS DAY CONVENTION'"/>
				</BUSINESSDAYCONVENTION>

				<CURRENCY1>
					<xsl:value-of select="'CURRENCY 1'"/>
				</CURRENCY1>


				<CURRENCY1AMOUNT>
					<xsl:value-of select="'CURRENCY 1 AMOUNT'"/>
				</CURRENCY1AMOUNT>


				<CURRENCY2>
					<xsl:value-of select="'CURRENCY 2'"/>
				</CURRENCY2>

				<CURRENCY2AMOUNT>
					<xsl:value-of select="'CURRENCY 2 AMOUNT'"/>
				</CURRENCY2AMOUNT>


				<FIXINGDATE>
					<xsl:value-of select="'FIXING DATE'"/>
				</FIXINGDATE>

				<SETTLEMENTTYPE>
					<xsl:value-of select="'SETTLEMENT TYPE'"/>
				</SETTLEMENTTYPE>

				<RESERVED>
					<xsl:value-of select="'RESERVED'"/>
				</RESERVED>


				<TERMINATIONDATE>
					<xsl:value-of select="'TERMINATION DATE'"/>
				</TERMINATIONDATE>

				<SWAPLEG1PRODUCTID>
					<xsl:value-of select="'SWAP LEG 1 PRODUCTID'"/>
				</SWAPLEG1PRODUCTID>

				<SWAPLEG1INDICATOR>
					<xsl:value-of select="'SWAP LEG 1 INDICATOR'"/>
				</SWAPLEG1INDICATOR>

				<SWAPLEG1TYPE>
					<xsl:value-of select="'SWAP LEG 1 TYPE'"/>
				</SWAPLEG1TYPE>

				<SWAPLEG1PAYMENTFREQUENCY>
					<xsl:value-of select="'SWAP LEG 1 PAYMENT FREQUENCY'"/>
				</SWAPLEG1PAYMENTFREQUENCY>

				<SWAPLEG1RATE>
					<xsl:value-of select="'SWAP LEG 1 RATE'"/>
				</SWAPLEG1RATE>


				<SWAPLEG1ACCRUALCONVENTION>
					<xsl:value-of select="'SWAP LEG 1 ACCRUAL CONVENTION'"/>
				</SWAPLEG1ACCRUALCONVENTION>

				<SWAPLEG2PRODUCTID>
					<xsl:value-of select="'SWAP LEG 2 PRODUCTID'"/>
				</SWAPLEG2PRODUCTID>

				<SWAPLEG2INDICATOR>
					<xsl:value-of select="'SWAP LEG 2 INDICATOR'"/>
				</SWAPLEG2INDICATOR>

				<SWAPLEG2TYPE>
					<xsl:value-of select="'SWAP LEG 2 TYPE'"/>
				</SWAPLEG2TYPE>

				<SWAPLEG2PAYMENTFREQUENCY>
					<xsl:value-of select="'SWAP LEG 2 PAYMENT FREQUENCY'"/>
				</SWAPLEG2PAYMENTFREQUENCY>


				<SWAPLEG2RATE>
					<xsl:value-of select="'SWAP LEG 2 RATE'"/>
				</SWAPLEG2RATE>


				<SWAPLEG2ACCRUALCONVENTION>
					<xsl:value-of select="'SWAP LEG 2 ACCRUAL CONVENTION'"/>
				</SWAPLEG2ACCRUALCONVENTION>


				<REDCODE>
					<xsl:value-of select="'RED CODE'"/>
				</REDCODE>

				<REFERENCEENTITY>
					<xsl:value-of select="'REFERENCE ENTITY'"/>
				</REFERENCEENTITY>

				<UNDERLYINGCUSIP>
					<xsl:value-of select="'UNDERLYING CUSIP'"/>
				</UNDERLYINGCUSIP>

				<UNDERLYINGISIN>
					<xsl:value-of select="'UNDERLYING ISIN'"/>
				</UNDERLYINGISIN>

				<RECOVERYRATE>
					<xsl:value-of select="'RECOVERY RATE'"/>
				</RECOVERYRATE>

				<RESERVED1>
					<xsl:value-of select="'RESERVED'"/>
				</RESERVED1>

				<RESERVED2>
					<xsl:value-of select="'RESERVED'"/>
				</RESERVED2>

				<RESERVED3>
					<xsl:value-of select="'RESERVED'"/>
				</RESERVED3>

				<RESERVED4>
					<xsl:value-of select="'RESERVED'"/>
				</RESERVED4>

				<RESERVED5>
					<xsl:value-of select="'RESERVED'"/>
				</RESERVED5>

				<RESERVED6>
					<xsl:value-of select="'RESERVED'"/>
				</RESERVED6>

				<RESERVED7>
					<xsl:value-of select="'RESERVED'"/>
				</RESERVED7>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = 'Heptagon - Shorts - Morgan Stanley']">
		
				<xsl:variable name="varNetamount">
					<xsl:choose>
						<xsl:when test="contains(Side,'Buy')">
							<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees  + OccFee + OrfFee"/>
						</xsl:when>
						<xsl:when test="contains(Side,'Sell')">
							<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees  + OccFee + OrfFee)"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test ="TaxLotState!='Amemded'">
						<ThirdPartyFlatFileDetail>

							<RowHeader>
								<xsl:value-of select ="'true'"/>
							</RowHeader>

							<FileHeader>
								<xsl:value-of select="'true'"/>
							</FileHeader>

						
							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<PRODUCTID>
								<xsl:value-of select="BBCode"/>
							</PRODUCTID>

							<TRADEID>
								<xsl:value-of select="EntityID"/>
							</TRADEID>

							<TRANSACTION>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'Buy to cvr'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short'">
										<xsl:value-of select="'Short sell '"/>
									</xsl:when>
								</xsl:choose>
							</TRANSACTION>

							<TRADESTATUS>								
								<xsl:choose>
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'NEWM'"/>
									</xsl:when>

									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'CANC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'SENT'"/>
									</xsl:otherwise>
								</xsl:choose>						
							</TRADESTATUS>



							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<TRADEDATE>
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</TRADEDATE>


							<SETTLEDATE>
								<xsl:value-of select="concat(substring-before($SettlementDate,'/'),'/',substring-before(substring-after($SettlementDate,'/'),'/'),'/',substring-after(substring-after($SettlementDate,'/'),'/'))"/>
							</SETTLEDATE>
							
							<xsl:variable name="PB_NAME" select="''"/>
							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<BROKER>
								<xsl:value-of select="CounterParty"/>
							</BROKER>

							<CUSTODIAN>
								<xsl:value-of select="'MSCO'"/>
							</CUSTODIAN>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>


							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ACCOUNTCODE>
								<xsl:value-of select="'HEPMS'"/>
							</ACCOUNTCODE>

							<QUANTITY>
								<xsl:value-of select="OrderQty"/>
							</QUANTITY>

							<AVERAGEPRICE>
								<xsl:value-of select="format-number(AvgPrice,'##.####')"/>
							</AVERAGEPRICE>

							<TOTALSECFEES>
								<xsl:value-of select="''"/>
							</TOTALSECFEES>

							<xsl:variable name="Otherfees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees  + OccFee + OrfFee"/>
							</xsl:variable>

							<TOTALNONSECFEES>
								<xsl:value-of select="$Otherfees"/>
							</TOTALNONSECFEES>
							
							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>
							<TOTALCOMMISSION>
								<xsl:value-of select="format-number($varCommission,'##.##')"/>
							</TOTALCOMMISSION>

							<ACCRUEDINTEREST>
								<xsl:value-of select="''"/>
							</ACCRUEDINTEREST>

							<NETAMOUNT>								
						     	<xsl:value-of select="format-number($varNetamount,'##.##')"/>									
							</NETAMOUNT>

							<SETTLEMENTCURRENCY>
								<xsl:value-of select="SettlCurrency"/>
							</SETTLEMENTCURRENCY>

							<TAXLOTID>
								<xsl:value-of select="''"/>
							</TAXLOTID>

							<VERSUSDATE>
								<xsl:value-of select="''"/>
							</VERSUSDATE>

							<STRATEGY1>
								<xsl:value-of select="''"/>
							</STRATEGY1>


							<STRATEGY2>
								<xsl:value-of select="''"/>
							</STRATEGY2>

							<STRATEGY3>
								<xsl:value-of select="''"/>
							</STRATEGY3>

							<STRATEGY4>
								<xsl:value-of select="''"/>
							</STRATEGY4>

							<STRATEGY5>
								<xsl:value-of select="''"/>
							</STRATEGY5>

							<SECURITYDESCRIPTION>
								<xsl:value-of select="CompanyName"/>
							</SECURITYDESCRIPTION>

							<SECURITIYCURRENCY>
								<xsl:value-of select="CurrencySymbol"/>
							</SECURITIYCURRENCY>

							<SECURITYTYPE>
								<xsl:value-of select="'SWAP'"/>
							</SECURITYTYPE>

							<MULTIPLIER>
								<xsl:value-of select="Multiplier"/>
							</MULTIPLIER>

							<OTCINDICATOR>
								<xsl:value-of select="''"/>
							</OTCINDICATOR>

							<SEDOL>
								<xsl:value-of select="SEDOL"/>
							</SEDOL>

							<ISIN>
								<xsl:value-of select="ISIN"/>
							</ISIN>

							<CUSIP>
								<xsl:value-of select="CUSIP"/>
							</CUSIP>

							<SECTOR>
								<xsl:value-of select="''"/>
							</SECTOR>


							<INDUSTRY>
								<xsl:value-of select="''"/>
							</INDUSTRY>

							<EXCHANGE>
								<xsl:value-of select="''"/>
							</EXCHANGE>


							<OCCCODE>
								<xsl:value-of select="''"/>
							</OCCCODE>

							<UNDERLYINGSECURITY>
								<xsl:value-of select="''"/>
							</UNDERLYINGSECURITY>

							<STRIKEPRICE>
								<xsl:value-of select="''"/>
							</STRIKEPRICE>

							<PUTCALL>
								<xsl:value-of select="''"/>
							</PUTCALL>

							<EXERCISESTYLE>
								<xsl:value-of select="''"/>
							</EXERCISESTYLE>


							<EXPIRATIONDATE>
								<xsl:value-of select="''"/>
							</EXPIRATIONDATE>

							<MATURITYTYPE>
								<xsl:value-of select="''"/>
							</MATURITYTYPE>

							<MATURITY>
								<xsl:value-of select="''"/>
							</MATURITY>

							<PARAMOUNT>
								<xsl:value-of select="''"/>
							</PARAMOUNT>


							<ISSUEDATE>
								<xsl:value-of select="''"/>
							</ISSUEDATE>

							<EFFECTIVEDATE>
								<xsl:value-of select="''"/>
							</EFFECTIVEDATE>


							<FIRSTPAYMENTDATE>
								<xsl:value-of select="''"/>
							</FIRSTPAYMENTDATE>

							<COUPONRATE>
								<xsl:value-of select="''"/>
							</COUPONRATE>

							<COUPONFREQUENCY>
								<xsl:value-of select="''"/>
								
							</COUPONFREQUENCY>

							<ACCRUALMETHOD>
								<xsl:value-of select="''"/>
							</ACCRUALMETHOD>

							<DEBTTYPE>
								<xsl:value-of select="''"/>
							</DEBTTYPE>


							<BUSINESSDAYCONVENTION>
								<xsl:value-of select="''"/>
							</BUSINESSDAYCONVENTION>

							<CURRENCY1>
								<xsl:value-of select="''"/>
							</CURRENCY1>


							<CURRENCY1AMOUNT>
								<xsl:value-of select="''"/>
							</CURRENCY1AMOUNT>


							<CURRENCY2>
								<xsl:value-of select="''"/>
							</CURRENCY2>

							<CURRENCY2AMOUNT>
								<xsl:value-of select="''"/>
							</CURRENCY2AMOUNT>


							<FIXINGDATE>
								<xsl:value-of select="''"/>
							</FIXINGDATE>

							<SETTLEMENTTYPE>
								<xsl:value-of select="''"/>
							</SETTLEMENTTYPE>

							<RESERVED>
								<xsl:value-of select="''"/>
							</RESERVED>


							<TERMINATIONDATE>
								<xsl:value-of select="''"/>
							</TERMINATIONDATE>

							<SWAPLEG1PRODUCTID>
								<xsl:value-of select="''"/>
							</SWAPLEG1PRODUCTID>

							<SWAPLEG1INDICATOR>
								<xsl:value-of select="''"/>
							</SWAPLEG1INDICATOR>

							<SWAPLEG1TYPE>
								<xsl:value-of select="''"/>
							</SWAPLEG1TYPE>

							<SWAPLEG1PAYMENTFREQUENCY>
								<xsl:value-of select="''"/>
							</SWAPLEG1PAYMENTFREQUENCY>

							<SWAPLEG1RATE>
								<xsl:value-of select="''"/>
							</SWAPLEG1RATE>


							<SWAPLEG1ACCRUALCONVENTION>
								<xsl:value-of select="''"/>
							</SWAPLEG1ACCRUALCONVENTION>

							<SWAPLEG2PRODUCTID>
								<xsl:value-of select="''"/>
							</SWAPLEG2PRODUCTID>


							<SWAPLEG2INDICATOR>
								<xsl:value-of select="''"/>
							</SWAPLEG2INDICATOR>

							<SWAPLEG2TYPE>
								<xsl:value-of select="''"/>
							</SWAPLEG2TYPE>

							<SWAPLEG2PAYMENTFREQUENCY>
								<xsl:value-of select="''"/>
							</SWAPLEG2PAYMENTFREQUENCY>


							<SWAPLEG2RATE>
								<xsl:value-of select="''"/>
							</SWAPLEG2RATE>


							<SWAPLEG2ACCRUALCONVENTION>
								<xsl:value-of select="''"/>
							</SWAPLEG2ACCRUALCONVENTION>


							<REDCODE>
								<xsl:value-of select="''"/>
							</REDCODE>

							<REFERENCEENTITY>
								<xsl:value-of select="''"/>
							</REFERENCEENTITY>

							<UNDERLYINGCUSIP>
								<xsl:value-of select="''"/>
							</UNDERLYINGCUSIP>

							<UNDERLYINGISIN>
								<xsl:value-of select="''"/>
							</UNDERLYINGISIN>

							<RECOVERYRATE>
								<xsl:value-of select="''"/>
							</RECOVERYRATE>

							<RESERVED1>
								<xsl:value-of select="''"/>
							</RESERVED1>

							<RESERVED2>
								<xsl:value-of select="''"/>
							</RESERVED2>

							<RESERVED3>
								<xsl:value-of select="''"/>
							</RESERVED3>

							<RESERVED4>
								<xsl:value-of select="''"/>
							</RESERVED4>

							<RESERVED5>
								<xsl:value-of select="''"/>
							</RESERVED5>

							<RESERVED6>
								<xsl:value-of select="''"/>
							</RESERVED6>

							<RESERVED7>
								<xsl:value-of select="''"/>
							</RESERVED7>


							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:when>

					<xsl:otherwise>
						<xsl:if test ="number(OldExecutedQuantity)">
							<ThirdPartyFlatFileDetail>

								<RowHeader>
									<xsl:value-of select ="'false'"/>
								</RowHeader>
								
								<FileHeader>
									<xsl:value-of select="'true'"/>
								</FileHeader>
								
								<TaxLotState>
									<xsl:value-of select="'Deleted'"/>
								</TaxLotState>

								<PRODUCTID>
									<xsl:value-of select="BBCode"/>
								</PRODUCTID>

								<TRADEID>
									<xsl:value-of select="EntityID"/>
								</TRADEID>

								<TRANSACTION>
									<xsl:choose>
										<xsl:when test="OldSide='Buy'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy to Close'">
											<xsl:value-of select="'Buy to cvr'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell short'">
											<xsl:value-of select="'Short sell '"/>
										</xsl:when>
									</xsl:choose>
								</TRANSACTION>

								<TRADESTATUS>
									<xsl:value-of select ="'CANC'"/>
								</TRADESTATUS>



								<xsl:variable name="OldTradeDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldTradeDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<xsl:variable name="OldSettleDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldSettlementDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<TRADEDATE>
									<xsl:value-of select="concat(substring-before($OldTradeDate,'/'),'/',substring-before(substring-after($OldTradeDate,'/'),'/'),'/',substring-after(substring-after($OldTradeDate,'/'),'/'))"/>
								</TRADEDATE>


								<SETTLEDATE>
									<xsl:value-of select="concat(substring-before($OldSettleDate,'/'),'/',substring-before(substring-after($OldSettleDate,'/'),'/'),'/',substring-after(substring-after($OldSettleDate,'/'),'/'))"/>
								</SETTLEDATE>

								

								<xsl:variable name="PB_NAME" select="''"/>
								<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
								<xsl:variable name="THIRDPARTY_BROKER">
									<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
								</xsl:variable>
								<BROKER>
									<xsl:value-of select="'MSSW'"/>
								</BROKER>

								<CUSTODIAN>
									<xsl:value-of select="'MSCO'"/>
								</CUSTODIAN>

								<xsl:variable name = "PRANA_FUND_NAME">
									<xsl:value-of select="''"/>
								</xsl:variable>

								<xsl:variable name ="THIRDPARTY_FUND_CODE">
									<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
								</xsl:variable>


								<xsl:variable name="varAccountName">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
											<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_FUND_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<ACCOUNTCODE>
									<xsl:value-of select="'HEPMS'"/>
								</ACCOUNTCODE>

								<QUANTITY>
									<xsl:value-of select="OldExecutedQuantity"/>
								</QUANTITY>

								<AVERAGEPRICE>
									<xsl:value-of select="format-number(OldAvgPrice,'##.####')"/>
								</AVERAGEPRICE>

								<TOTALSECFEES>
									<xsl:value-of select="''"/>
								</TOTALSECFEES>

								<xsl:variable name="OldOtherFees">
									<xsl:value-of select="(OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldOccFee + OldOrfFee)"/>
								</xsl:variable>
								<TOTALNONSECFEES>
									<xsl:value-of select="$OldOtherFees"/>
								</TOTALNONSECFEES>

								<xsl:variable name="varTotalCommission">
									<xsl:value-of select="(OldCommission + OldSoftCommission)"/>
								</xsl:variable>
								<TOTALCOMMISSION>
									<xsl:value-of select="format-number($varTotalCommission,'##.##')"/>
								</TOTALCOMMISSION>

								<ACCRUEDINTEREST>
									<xsl:value-of select="''"/>
								</ACCRUEDINTEREST>
								
								<xsl:variable name="varOldNetAmount">
									<xsl:choose>
										<xsl:when test="contains(Side,'Buy')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees  + OldOccFee + OldOrfFee"/>
										</xsl:when>
										<xsl:when test="contains(Side,'Sell')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldOccFee + OldOrfFee)"/>
										</xsl:when>
									</xsl:choose>
								</xsl:variable>
								<NETAMOUNT>
									<xsl:value-of select="format-number($varOldNetAmount,'##.##')"/>
								</NETAMOUNT>

								<SETTLEMENTCURRENCY>
									<xsl:value-of select="'USD'"/>
								</SETTLEMENTCURRENCY>

								<TAXLOTID>
									<xsl:value-of select="''"/>
								</TAXLOTID>

								<VERSUSDATE>
									<xsl:value-of select="''"/>
								</VERSUSDATE>

								<STRATEGY1>
									<xsl:value-of select="''"/>
								</STRATEGY1>


								<STRATEGY2>
									<xsl:value-of select="''"/>
								</STRATEGY2>

								<STRATEGY3>
									<xsl:value-of select="''"/>
								</STRATEGY3>

								<STRATEGY4>
									<xsl:value-of select="''"/>
								</STRATEGY4>

								<STRATEGY5>
									<xsl:value-of select="''"/>
								</STRATEGY5>

								<SECURITYDESCRIPTION>
									<xsl:value-of select="CompanyName"/>
								</SECURITYDESCRIPTION>

								<SECURITIYCURRENCY>
									<xsl:value-of select="CurrencySymbol"/>
								</SECURITIYCURRENCY>

								<SECURITYTYPE>
									<xsl:value-of select="'SWAP'"/>
								</SECURITYTYPE>

								<MULTIPLIER>
									<xsl:value-of select="Multiplier"/>
								</MULTIPLIER>

								<OTCINDICATOR>
									<xsl:value-of select="''"/>
								</OTCINDICATOR>

								<SEDOL>
									<xsl:value-of select="SEDOL"/>
								</SEDOL>

								<ISIN>
									<xsl:value-of select="ISIN"/>
								</ISIN>

								<CUSIP>
									<xsl:value-of select="CUSIP"/>
								</CUSIP>

								<SECTOR>
									<xsl:value-of select="''"/>
								</SECTOR>


								<INDUSTRY>
									<xsl:value-of select="''"/>
								</INDUSTRY>

								<EXCHANGE>
									<xsl:value-of select="''"/>
								</EXCHANGE>


								<OCCCODE>
									<xsl:value-of select="''"/>
								</OCCCODE>

								<UNDERLYINGSECURITY>
									<xsl:value-of select="''"/>
								</UNDERLYINGSECURITY>

								<STRIKEPRICE>
									<xsl:value-of select="''"/>
								</STRIKEPRICE>

								<PUTCALL>
									<xsl:value-of select="''"/>
								</PUTCALL>

								<EXERCISESTYLE>
									<xsl:value-of select="''"/>
								</EXERCISESTYLE>


								<EXPIRATIONDATE>
									<xsl:value-of select="''"/>
								</EXPIRATIONDATE>

								<MATURITYTYPE>
									<xsl:value-of select="''"/>
								</MATURITYTYPE>

								<MATURITY>
									<xsl:value-of select="''"/>
								</MATURITY>

								<PARAMOUNT>
									<xsl:value-of select="''"/>
								</PARAMOUNT>


								<ISSUEDATE>
									<xsl:value-of select="''"/>
								</ISSUEDATE>

								<EFFECTIVEDATE>
									<xsl:value-of select="''"/>
								</EFFECTIVEDATE>


								<FIRSTPAYMENTDATE>
									<xsl:value-of select="''"/>
								</FIRSTPAYMENTDATE>

								<COUPONRATE>
									<xsl:value-of select="''"/>
								</COUPONRATE>

								<COUPONFREQUENCY>
									<xsl:value-of select="''"/>

								</COUPONFREQUENCY>

								<ACCRUALMETHOD>
									<xsl:value-of select="''"/>
								</ACCRUALMETHOD>

								<DEBTTYPE>
									<xsl:value-of select="''"/>
								</DEBTTYPE>


								<BUSINESSDAYCONVENTION>
									<xsl:value-of select="''"/>
								</BUSINESSDAYCONVENTION>

								<CURRENCY1>
									<xsl:value-of select="''"/>
								</CURRENCY1>


								<CURRENCY1AMOUNT>
									<xsl:value-of select="''"/>
								</CURRENCY1AMOUNT>


								<CURRENCY2>
									<xsl:value-of select="''"/>
								</CURRENCY2>

								<CURRENCY2AMOUNT>
									<xsl:value-of select="''"/>
								</CURRENCY2AMOUNT>


								<FIXINGDATE>
									<xsl:value-of select="''"/>
								</FIXINGDATE>

								<SETTLEMENTTYPE>
									<xsl:value-of select="''"/>
								</SETTLEMENTTYPE>

								<RESERVED>
									<xsl:value-of select="''"/>
								</RESERVED>


								<TERMINATIONDATE>
									<xsl:value-of select="''"/>
								</TERMINATIONDATE>

								<SWAPLEG1PRODUCTID>
									<xsl:value-of select="''"/>
								</SWAPLEG1PRODUCTID>

								<SWAPLEG1INDICATOR>
									<xsl:value-of select="''"/>
								</SWAPLEG1INDICATOR>

								<SWAPLEG1TYPE>
									<xsl:value-of select="''"/>
								</SWAPLEG1TYPE>

								<SWAPLEG1PAYMENTFREQUENCY>
									<xsl:value-of select="''"/>
								</SWAPLEG1PAYMENTFREQUENCY>

								<SWAPLEG1RATE>
									<xsl:value-of select="''"/>
								</SWAPLEG1RATE>


								<SWAPLEG1ACCRUALCONVENTION>
									<xsl:value-of select="''"/>
								</SWAPLEG1ACCRUALCONVENTION>

								<SWAPLEG2PRODUCTID>
									<xsl:value-of select="''"/>
								</SWAPLEG2PRODUCTID>


								<SWAPLEG2INDICATOR>
									<xsl:value-of select="''"/>
								</SWAPLEG2INDICATOR>

								<SWAPLEG2TYPE>
									<xsl:value-of select="''"/>
								</SWAPLEG2TYPE>

								<SWAPLEG2PAYMENTFREQUENCY>
									<xsl:value-of select="''"/>
								</SWAPLEG2PAYMENTFREQUENCY>


								<SWAPLEG2RATE>
									<xsl:value-of select="''"/>
								</SWAPLEG2RATE>


								<SWAPLEG2ACCRUALCONVENTION>
									<xsl:value-of select="''"/>
								</SWAPLEG2ACCRUALCONVENTION>


								<REDCODE>
									<xsl:value-of select="''"/>
								</REDCODE>

								<REFERENCEENTITY>
									<xsl:value-of select="''"/>
								</REFERENCEENTITY>

								<UNDERLYINGCUSIP>
									<xsl:value-of select="''"/>
								</UNDERLYINGCUSIP>

								<UNDERLYINGISIN>
									<xsl:value-of select="''"/>
								</UNDERLYINGISIN>

								<RECOVERYRATE>
									<xsl:value-of select="''"/>
								</RECOVERYRATE>

								<RESERVED1>
									<xsl:value-of select="''"/>
								</RESERVED1>

								<RESERVED2>
									<xsl:value-of select="''"/>
								</RESERVED2>

								<RESERVED3>
									<xsl:value-of select="''"/>
								</RESERVED3>

								<RESERVED4>
									<xsl:value-of select="''"/>
								</RESERVED4>

								<RESERVED5>
									<xsl:value-of select="''"/>
								</RESERVED5>

								<RESERVED6>
									<xsl:value-of select="''"/>
								</RESERVED6>

								<RESERVED7>
									<xsl:value-of select="''"/>
								</RESERVED7>
								
								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>
						<ThirdPartyFlatFileDetail>
							<RowHeader>
								<xsl:value-of select ="'true'"/>
							</RowHeader>

							<FileHeader>
								<xsl:value-of select="'true'"/>
							</FileHeader>

							
							<TaxLotState>
								<xsl:value-of select="'Allocated'"/>
							</TaxLotState>
							
							<PRODUCTID>
								<xsl:value-of select="BBCode"/>
							</PRODUCTID>

							<TRADEID>
								<xsl:value-of select="EntityID"/>
							</TRADEID>

							<TRANSACTION>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'Buy to cvr'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short'">
										<xsl:value-of select="'Short sell '"/>
									</xsl:when>
								</xsl:choose>
							</TRANSACTION>

							<TRADESTATUS>								
								<xsl:value-of select ="'NEWM'"/> 									
							</TRADESTATUS>



							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<TRADEDATE>
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</TRADEDATE>


							<SETTLEDATE>
								<xsl:value-of select="concat(substring-before($SettlementDate,'/'),'/',substring-before(substring-after($SettlementDate,'/'),'/'),'/',substring-after(substring-after($SettlementDate,'/'),'/'))"/>
							</SETTLEDATE>

							<xsl:variable name="PB_NAME" select="''"/>
							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<BROKER>
								<xsl:value-of select="'MSSW'"/>
							</BROKER>

							<CUSTODIAN>
								<xsl:value-of select="'MSCO'"/>
							</CUSTODIAN>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>


							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ACCOUNTCODE>
								<xsl:value-of select="'HEPMS'"/>
							</ACCOUNTCODE>

							<QUANTITY>
								<xsl:value-of select="OrderQty"/>
							</QUANTITY>

							<AVERAGEPRICE>
								<xsl:value-of select="format-number(AvgPrice,'##.####')"/>
							</AVERAGEPRICE>

							<TOTALSECFEES>
								<xsl:value-of select="''"/>
							</TOTALSECFEES>

							<xsl:variable name="Otherfees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees  + OccFee + OrfFee"/>
							</xsl:variable>
							
							<TOTALNONSECFEES>
								<xsl:value-of select="$Otherfees"/>
							</TOTALNONSECFEES>

							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>
							<TOTALCOMMISSION>
								<xsl:value-of select="format-number($varCommission,'##.##')"/>
							</TOTALCOMMISSION>

							<ACCRUEDINTEREST>
								<xsl:value-of select="''"/>
							</ACCRUEDINTEREST>

							<NETAMOUNT>
								<xsl:value-of select="format-number($varNetamount,'##.##')"/>
							</NETAMOUNT>

							<SETTLEMENTCURRENCY>
								<xsl:value-of select="SettlCurrency"/>
							</SETTLEMENTCURRENCY>

							<TAXLOTID>
								<xsl:value-of select="''"/>
							</TAXLOTID>

							<VERSUSDATE>
								<xsl:value-of select="''"/>
							</VERSUSDATE>

							<STRATEGY1>
								<xsl:value-of select="''"/>
							</STRATEGY1>


							<STRATEGY2>
								<xsl:value-of select="''"/>
							</STRATEGY2>

							<STRATEGY3>
								<xsl:value-of select="''"/>
							</STRATEGY3>

							<STRATEGY4>
								<xsl:value-of select="''"/>
							</STRATEGY4>

							<STRATEGY5>
								<xsl:value-of select="''"/>
							</STRATEGY5>

							<SECURITYDESCRIPTION>
								<xsl:value-of select="CompanyName"/>
							</SECURITYDESCRIPTION>

							<SECURITIYCURRENCY>
								<xsl:value-of select="CurrencySymbol"/>
							</SECURITIYCURRENCY>

							<SECURITYTYPE>
								<xsl:value-of select="'SWAP'"/>
							</SECURITYTYPE>

							<MULTIPLIER>
								<xsl:value-of select="Multiplier"/>
							</MULTIPLIER>

							<OTCINDICATOR>
								<xsl:value-of select="''"/>
							</OTCINDICATOR>

							<SEDOL>
								<xsl:value-of select="SEDOL"/>
							</SEDOL>

							<ISIN>
								<xsl:value-of select="ISIN"/>
							</ISIN>

							<CUSIP>
								<xsl:value-of select="CUSIP"/>
							</CUSIP>

							<SECTOR>
								<xsl:value-of select="''"/>
							</SECTOR>


							<INDUSTRY>
								<xsl:value-of select="''"/>
							</INDUSTRY>

							<EXCHANGE>
								<xsl:value-of select="''"/>
							</EXCHANGE>


							<OCCCODE>
								<xsl:value-of select="''"/>
							</OCCCODE>

							<UNDERLYINGSECURITY>
								<xsl:value-of select="''"/>
							</UNDERLYINGSECURITY>

							<STRIKEPRICE>
								<xsl:value-of select="''"/>
							</STRIKEPRICE>

							<PUTCALL>
								<xsl:value-of select="''"/>
							</PUTCALL>

							<EXERCISESTYLE>
								<xsl:value-of select="''"/>
							</EXERCISESTYLE>


							<EXPIRATIONDATE>
								<xsl:value-of select="''"/>
							</EXPIRATIONDATE>

							<MATURITYTYPE>
								<xsl:value-of select="''"/>
							</MATURITYTYPE>

							<MATURITY>
								<xsl:value-of select="''"/>
							</MATURITY>

							<PARAMOUNT>
								<xsl:value-of select="''"/>
							</PARAMOUNT>


							<ISSUEDATE>
								<xsl:value-of select="''"/>
							</ISSUEDATE>

							<EFFECTIVEDATE>
								<xsl:value-of select="''"/>
							</EFFECTIVEDATE>


							<FIRSTPAYMENTDATE>
								<xsl:value-of select="''"/>
							</FIRSTPAYMENTDATE>

							<COUPONRATE>
								<xsl:value-of select="''"/>
							</COUPONRATE>

							<COUPONFREQUENCY>
								<xsl:value-of select="''"/>

							</COUPONFREQUENCY>

							<ACCRUALMETHOD>
								<xsl:value-of select="''"/>
							</ACCRUALMETHOD>

							<DEBTTYPE>
								<xsl:value-of select="''"/>
							</DEBTTYPE>


							<BUSINESSDAYCONVENTION>
								<xsl:value-of select="''"/>
							</BUSINESSDAYCONVENTION>

							<CURRENCY1>
								<xsl:value-of select="''"/>
							</CURRENCY1>


							<CURRENCY1AMOUNT>
								<xsl:value-of select="''"/>
							</CURRENCY1AMOUNT>


							<CURRENCY2>
								<xsl:value-of select="''"/>
							</CURRENCY2>

							<CURRENCY2AMOUNT>
								<xsl:value-of select="''"/>
							</CURRENCY2AMOUNT>


							<FIXINGDATE>
								<xsl:value-of select="''"/>
							</FIXINGDATE>

							<SETTLEMENTTYPE>
								<xsl:value-of select="''"/>
							</SETTLEMENTTYPE>

							<RESERVED>
								<xsl:value-of select="''"/>
							</RESERVED>


							<TERMINATIONDATE>
								<xsl:value-of select="''"/>
							</TERMINATIONDATE>

							<SWAPLEG1PRODUCTID>
								<xsl:value-of select="''"/>
							</SWAPLEG1PRODUCTID>

							<SWAPLEG1INDICATOR>
								<xsl:value-of select="''"/>
							</SWAPLEG1INDICATOR>

							<SWAPLEG1TYPE>
								<xsl:value-of select="''"/>
							</SWAPLEG1TYPE>

							<SWAPLEG1PAYMENTFREQUENCY>
								<xsl:value-of select="''"/>
							</SWAPLEG1PAYMENTFREQUENCY>

							<SWAPLEG1RATE>
								<xsl:value-of select="''"/>
							</SWAPLEG1RATE>


							<SWAPLEG1ACCRUALCONVENTION>
								<xsl:value-of select="''"/>
							</SWAPLEG1ACCRUALCONVENTION>

							<SWAPLEG2PRODUCTID>
								<xsl:value-of select="''"/>
							</SWAPLEG2PRODUCTID>


							<SWAPLEG2INDICATOR>
								<xsl:value-of select="''"/>
							</SWAPLEG2INDICATOR>

							<SWAPLEG2TYPE>
								<xsl:value-of select="''"/>
							</SWAPLEG2TYPE>

							<SWAPLEG2PAYMENTFREQUENCY>
								<xsl:value-of select="''"/>
							</SWAPLEG2PAYMENTFREQUENCY>


							<SWAPLEG2RATE>
								<xsl:value-of select="''"/>
							</SWAPLEG2RATE>


							<SWAPLEG2ACCRUALCONVENTION>
								<xsl:value-of select="''"/>
							</SWAPLEG2ACCRUALCONVENTION>


							<REDCODE>
								<xsl:value-of select="''"/>
							</REDCODE>

							<REFERENCEENTITY>
								<xsl:value-of select="''"/>
							</REFERENCEENTITY>

							<UNDERLYINGCUSIP>
								<xsl:value-of select="''"/>
							</UNDERLYINGCUSIP>

							<UNDERLYINGISIN>
								<xsl:value-of select="''"/>
							</UNDERLYINGISIN>

							<RECOVERYRATE>
								<xsl:value-of select="''"/>
							</RECOVERYRATE>

							<RESERVED1>
								<xsl:value-of select="''"/>
							</RESERVED1>

							<RESERVED2>
								<xsl:value-of select="''"/>
							</RESERVED2>

							<RESERVED3>
								<xsl:value-of select="''"/>
							</RESERVED3>

							<RESERVED4>
								<xsl:value-of select="''"/>
							</RESERVED4>

							<RESERVED5>
								<xsl:value-of select="''"/>
							</RESERVED5>

							<RESERVED6>
								<xsl:value-of select="''"/>
							</RESERVED6>

							<RESERVED7>
								<xsl:value-of select="''"/>
							</RESERVED7>
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>

					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
