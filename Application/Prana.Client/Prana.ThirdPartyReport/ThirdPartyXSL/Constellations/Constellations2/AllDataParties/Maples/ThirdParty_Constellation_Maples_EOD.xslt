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

	<xsl:template name="Conversion">
		<xsl:param name="Value"/>
		<xsl:param name="Curr"/>

			
				<xsl:choose>
					<xsl:when test="FXConversionMethodOperator_Taxlot='M'">
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="$Value * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD' or $Curr='TRY'">
										<xsl:value-of select="$Value * ForexRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$Value div ForexRate"/>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="$Value div FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD' or $Curr='TRY'">
										<xsl:value-of select="$Value * ForexRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$Value div ForexRate"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
		

	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='GaoHui Fund-Swaps' or AccountName='101L2141' or AccountName='1740-PRIM-NIP' or AccountName='GaoHui Fund-Cash Equity' or AccountName='CPB20740']">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="TAction">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'NEW'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select="'CORRECT'"/>
							</xsl:when>

							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'DELETE'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<TradeAction>
						<xsl:value-of select="$TAction"/>
					</TradeAction>

					

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<TranType>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
								<xsl:value-of select="'BuytoClose'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SellShort'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TranType>

					<InvestID>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="contains(substring-after(BBCode,' '),' ')">
										<xsl:value-of select="concat(substring-before(BBCode,' '),' ',substring-before(substring-after(BBCode,' '),' '))"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat(substring-before(BBCode,' '),' ',substring-after(BBCode,' '))"/>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:otherwise>
						</xsl:choose>
					</InvestID>

					<Investment>
						<xsl:value-of select="FullSecurityName"/>
					</Investment>

					<InvestmentType>
						<xsl:choose>
							<xsl:when test="AccountName='GAOHUI FUND SPB USD COMP'">
								<xsl:value-of select="'Swap'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</InvestmentType>

					<CustodianAccount>
						<xsl:value-of select="''"/>
					</CustodianAccount>

					<Quantity>
						<xsl:choose>
							<xsl:when test="AllocatedQty &gt; 0">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:when test="AllocatedQty &lt; 0">
								<xsl:value-of select="AllocatedQty * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>

					<Price>
						<xsl:choose>
							<xsl:when test="AveragePrice &gt; 0">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:when test="AveragePrice &lt; 0">
								<xsl:value-of select="AveragePrice * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Price>

					<SEC>
						<xsl:choose>
							<xsl:when test="StampDuty &gt; 0">
								<xsl:value-of select="StampDuty"/>
							</xsl:when>
							<xsl:when test="StampDuty &lt; 0">
								<xsl:value-of select="StampDuty * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</SEC>

					<xsl:variable name="LocAmount" select="AllocatedQty * AveragePrice"/>

					<LocalAmount>
						<xsl:choose>
							<xsl:when test="$LocAmount &gt; 0">
								<xsl:value-of select="$LocAmount"/>
							</xsl:when>
							<xsl:when test="$LocAmount &lt; 0">
								<xsl:value-of select="$LocAmount * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</LocalAmount>

					<BookAmount>
						<xsl:call-template name="Conversion">
							<xsl:with-param name="Value" select="$LocAmount"/>
							<xsl:with-param name="Curr" select="CurrencySymbol"/>
						</xsl:call-template>
					</BookAmount>

					<ContractDate>
						<xsl:value-of select="SettlementDate"/>
					</ContractDate>

					<TranID>
						<xsl:value-of select="EntityID"/>
					</TranID>

					<GenericInvestment>
						<xsl:value-of select="BBCode"/>
					</GenericInvestment>

					<xsl:variable name="PB_NAME" select="'Maples'"/>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="PB_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<Broker>
						<xsl:choose>
							<xsl:when test="$PB_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Broker>

					<Trader>
						<xsl:value-of select="''"/>
					</Trader>

					<Commission>
						<xsl:choose>
							<xsl:when test="CommissionCharged &gt; 0">
								<xsl:value-of select="CommissionCharged"/>
							</xsl:when>
							<xsl:when test="CommissionCharged &lt; 0">
								<xsl:value-of select="CommissionCharged * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>

					<xsl:variable name="Fees" select="OtherBrokerFee + ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>

					<Expenses>
						<xsl:choose>
							<xsl:when test="$Fees &gt; 0">
								<xsl:value-of select="$Fees"/>
							</xsl:when>
							<xsl:when test="$Fees &lt; 0">
								<xsl:value-of select="$Fees * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Expenses>

					<LocalCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</LocalCurrency>

					<xsl:variable name="TotalAmount" select="$LocAmount + StampDuty + CommissionCharged + $Fees"/>

					<TotalBookAmount>
						<xsl:call-template name="Conversion">
							<xsl:with-param name="Value" select="$TotalAmount"/>
							<xsl:with-param name="Curr" select="CurrencySymbol"/>
						</xsl:call-template>
					</TotalBookAmount>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>