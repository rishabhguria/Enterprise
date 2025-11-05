<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public int RoundOff(double Qty)
		{
		
		return (int)Math.Round(Qty,0);
		}
	</msxsl:script>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>


				<IsCaptionChangeRequired>
					<xsl:value-of select ="'false'"/>
				</IsCaptionChangeRequired>

				<FileHeader>
					<xsl:value-of select="'true'"/>
				</FileHeader>

				<FileFooter>
					<xsl:value-of select="'false'"/>
				</FileFooter>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxLotState>


				<CUSIP>
					<xsl:value-of select="concat('&#x0a;','CUSIP')"/>
				</CUSIP>

				<CODE>
					<xsl:value-of select="'CODE'"/>
				</CODE>

				<UNITS>
					<xsl:value-of select="'UNITS'"/>
				</UNITS>

				<PRSHR>
					<xsl:value-of select="'PRSHR'"/>
				</PRSHR>

				<BROKER>
					<xsl:value-of select="'BROKER'"/>
				</BROKER>

				<TRADDT>
					<xsl:value-of select="'TRADDT'"/>
				</TRADDT>

				<CONTDT>
					<xsl:value-of select="'CONTDT'"/>
				</CONTDT>

				<COMMS>
					<xsl:value-of select="'COMMS'"/>
				</COMMS>

				<SECFEES>
					<xsl:value-of select="'S.E.C. FEES'"/>
				</SECFEES>

				<NET>
					<xsl:value-of select="'NET'"/>
				</NET>

				<!--<Currency>
					<xsl:value-of select="'Currency'"/>
				</Currency>-->

				<TICKER>
					<xsl:value-of select="'TICKER'"/>
				</TICKER>

				<SECURITYDESCRIPTION>
					<xsl:value-of select="'SECURITY DESCRIPTION'"/>
				</SECURITYDESCRIPTION>

				<!--<TradeSettlement>
					<xsl:value-of select="'Trade Settlement'"/>
				</TradeSettlement>-->

				<!--<AccountName>
					<xsl:value-of select="'Account Name:'"/>
				</AccountName>-->

				<!--<AccountNumber>
					<xsl:value-of select="'Account Number:'"/>
				</AccountNumber>-->


				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>


			<!--<xsl:for-each select="ThirdPartyFlatFileDetail[FundName='Cedars-Sinai Medical Center: 6745039604' or FundName='LYRIX-000000000000940' or FundName='LYRHX-000000000000941' or FundName='USO Foundation: 105099011' ]">-->
			<!--<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='USO Foundation: 105099011']">-->
				<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>


					<IsCaptionChangeRequired>
						<xsl:value-of select ="'false'"/>
					</IsCaptionChangeRequired>

					<FileHeader>
						<xsl:value-of select="'true'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select="'false'"/>
					</FileFooter>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>
					<CUSIP>
						<xsl:choose>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="concat(concat('=&quot;',CUSIP),'&quot;')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CUSIP>

					<CODE>
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select="'SELL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short'">
								<xsl:value-of select="'Sell Short'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</CODE>

					<xsl:variable name ="Qty">
						<xsl:value-of select="my:RoundOff(AllocatedQty)"/>
					</xsl:variable>

					<UNITS>
						<xsl:choose>
							<xsl:when test="number($Qty)">
								<xsl:value-of select="$Qty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</UNITS>

					<xsl:variable name ="varAveragePrice">
							<xsl:choose>
								<xsl:when test="number(AveragePrice)">
									<xsl:value-of select="format-number(AveragePrice,'0.0000')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
					</xsl:variable>
					<PRSHR>
						<xsl:value-of select="$varAveragePrice"/>
					</PRSHR>

					<xsl:variable name="PB_NAME" select="'Lyrical'"/>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<BROKER>
						<xsl:choose>
							<xsl:when test="CounterParty='JPM'">
								<xsl:value-of select="'JPM Securities LLC'"/>
							</xsl:when>
							<xsl:when test="CounterParty='BGCE'">
								<xsl:value-of select="'Merrill Lynch Broadcort'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMHI'">
								<xsl:value-of select="'Sanders Morris Harris LLC'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMBC'">
								<xsl:value-of select="'SMBC Nikko Securities America Inc'"/>
							</xsl:when>
							<xsl:when test="CounterParty='BARC'">
								<xsl:value-of select="'Barclays PLC'"/>
							</xsl:when>

						</xsl:choose>
					</BROKER>

					<TRADDT>
						<xsl:value-of select="TradeDate"/>
					</TRADDT>

					<CONTDT>
						<xsl:value-of select="SettlementDate"/>
					</CONTDT>

					<xsl:variable name="Commission1">
						<xsl:value-of select="SoftCommissionCharged + CommissionCharged"/>
					</xsl:variable>

					<xsl:variable name="varCommission">
						<xsl:choose>
							<xsl:when test="number($Commission1)">
								<xsl:value-of select="format-number($Commission1,'0.##')"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<COMMS>
						<xsl:value-of select="$varCommission"/>
					</COMMS>

					<xsl:variable name="varStampDuty">
						<xsl:value-of select="format-number(StampDuty,'0.##')"/>
					</xsl:variable>
					<SECFEES>
						<xsl:value-of select="$varStampDuty"/>
					</SECFEES>

					<xsl:variable name="varNetAmount">
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="$varAveragePrice * $Qty + $varCommission + $varStampDuty"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select="$varAveragePrice * $Qty - $varCommission - $varStampDuty"/>
							</xsl:when>
							<xsl:when test="Side='Sell short'">
								<xsl:value-of select="$varAveragePrice * $Qty - $varCommission - $varStampDuty"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="$varAveragePrice * $Qty + $varCommission + $varStampDuty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>
					
					<NET>
						<xsl:choose>
							<xsl:when test="number($varNetAmount)">
								<xsl:value-of select="format-number($varNetAmount,'0.##')"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</NET>

					<!--<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>-->

					<TICKER>
						<xsl:value-of select="Symbol"/>
					</TICKER>

					<SECURITYDESCRIPTION>
						<xsl:value-of select="FullSecurityName"/>
					</SECURITYDESCRIPTION>

					<!--<TradeSettlement>
						<xsl:choose>
							<xsl:when test="CounterParty='JPM'">
								<xsl:value-of select="'DTC 352'"/>
							</xsl:when>
							<xsl:when test="CounterParty='BGCE'">
								<xsl:value-of select="'DTC 161'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMHI'">
								<xsl:value-of select="'DTC 443'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMBC'">
								<xsl:value-of select="'DTC 443'"/>
							</xsl:when>

						</xsl:choose>
					</TradeSettlement>-->

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="PB_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name='EOD']/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/>
					</xsl:variable>

					<!--<AccountName>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountName>-->

					<!--<AccountNumber>
						--><!--<xsl:choose>
							<xsl:when test="FundName='Cedars-Sinai Medical Center: 6745039604'">
								--><!--<xsl:value-of select="'6745039604'"/>--><!--
								<xsl:value-of select="concat(concat('=&quot;','6745039604'),'&quot;')"/>
							</xsl:when>

							<xsl:when test="FundName='LYRIX-000000000000940'">
								--><!--<xsl:value-of select="'000000000000940'"/>--><!--
								<xsl:value-of select="concat(concat('=&quot;','000000000000940'),'&quot;')"/>
							</xsl:when>

							<xsl:when test="FundName='LYRHX-000000000000941'">
								--><!--<xsl:value-of select="'19-4561'"/>--><!--
								<xsl:value-of select="concat(concat('=&quot;','19-4561'),'&quot;')"/>
							</xsl:when>

							<xsl:when test="FundName='USO Foundation: 105099011'">
								<xsl:value-of select="concat(concat('=&quot;','001050990011'),'&quot;')"/>
								--><!--<xsl:value-of select="'001050990011'"/>--><!--
							</xsl:when>

							
							<xsl:otherwise>--><!--
								<xsl:choose>
									<xsl:when test ="$PB_FUND_CODE!=''">
										<xsl:value-of select ="$PB_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="''"/>
									</xsl:otherwise>
								</xsl:choose>
							--><!--</xsl:otherwise>
						</xsl:choose>--><!--

					</AccountNumber>-->

					<EntityID>
						<xsl:value-of select="''"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
