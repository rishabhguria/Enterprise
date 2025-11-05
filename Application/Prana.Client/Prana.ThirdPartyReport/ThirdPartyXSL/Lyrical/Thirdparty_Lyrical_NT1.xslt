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
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<FileHeader>
					<xsl:value-of select="'true'"/>
				</FileHeader>

				<FileFooter>
					<xsl:value-of select="'true'"/>
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

				<Currency>
					<xsl:value-of select="'Currency'"/>
				</Currency>

				<TICKER>
					<xsl:value-of select="'TICKER'"/>
				</TICKER>

				<SECURITYDESCRIPTION>
					<xsl:value-of select="'SECURITY DESCRIPTION'"/>
				</SECURITYDESCRIPTION>

				<TradeSettlement>
					<xsl:value-of select="'Trade Settlement'"/>
				</TradeSettlement>

				<!--<AccountName>
					<xsl:value-of select="'Account Name:'"/>
				</AccountName>-->

				<AccountNumber>
					<xsl:value-of select="'Account Number:'"/>
				</AccountNumber>


				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>


			
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>


					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<FileHeader>
						<xsl:value-of select="'true'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select="'true'"/>
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

					<PRSHR>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
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

					<COMMS>
						<xsl:choose>
							<xsl:when test="number($Commission1)">
								<xsl:value-of select="format-number($Commission1,'0.##')"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:value-of select="$Commission1"/>-->
					</COMMS>

					<SECFEES>
						<xsl:value-of select="format-number(StampDuty,'0.##')"/>
					</SECFEES>

					<NET>
						<xsl:choose>
							<xsl:when test="number(NetAmount)">
								<xsl:value-of select="format-number(NetAmount,'0.##')"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</NET>

					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>

					<TICKER>
						<xsl:value-of select="Symbol"/>
					</TICKER>

					<SECURITYDESCRIPTION>
						<xsl:value-of select="FullSecurityName"/>
					</SECURITYDESCRIPTION>

					<TradeSettlement>
						<xsl:choose>
							<xsl:when test="CounterParty='JPM'">
								<xsl:value-of select="'DTC 352'"/>
							</xsl:when>
							<xsl:when test="CounterParty='BGCE'">
								<xsl:value-of select="'DTC 671'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMHI'">
								<xsl:value-of select="'DTC 443'"/>
							</xsl:when>

						</xsl:choose>
					</TradeSettlement>

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

					<AccountNumber>
						
								<xsl:choose>
									<xsl:when test ="$PB_FUND_CODE!=''">
										<xsl:value-of select ="$PB_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="''"/>
									</xsl:otherwise>
								</xsl:choose>
						

					</AccountNumber>

					<EntityID>
						<xsl:value-of select="''"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
