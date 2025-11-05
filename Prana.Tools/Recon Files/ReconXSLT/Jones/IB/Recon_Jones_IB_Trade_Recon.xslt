<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL3"/>
					</xsl:call-template>
				</xsl:variable>
				<!--<xsl:if test="number($Quantity) and COL7!='Cash'">-->
				<PositionMaster>
					<xsl:variable name="PB_NAME">
						<xsl:value-of select="'Interactive Broker'"/>
					</xsl:variable>
					<xsl:variable name = "PB_COMPANY_NAME" >
						<xsl:value-of select="COL8"/>
					</xsl:variable>
					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
					</xsl:variable>
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL1"/>
					</xsl:variable>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>
					<AccountName>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME!=''">
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PB_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountName>
					<xsl:variable name ="Asset">
						<xsl:choose>
							<xsl:when test=" COL6='EquityOption' and string-length(COL10 &gt; 20)">
								<xsl:value-of select="'EquityOption'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Equity'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="Symbol">
						<xsl:value-of select="COL7"/>
					</xsl:variable>
					<Symbol>
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							</xsl:when>
							<xsl:when test="$Asset='EquityOption'">
								<xsl:value-of select="''"/>
							</xsl:when>

							<xsl:when test="$Symbol!='*'">
								<xsl:value-of select="$Symbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PB_COMPANY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>
					<IDCOOptionSymbol>
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:when test="$Asset='EquityOption'">
								<xsl:value-of select="concat(COL6,'U')"/>
							</xsl:when>
							<xsl:when test="$Symbol!='*'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</IDCOOptionSymbol>


					<Quantity>
						<xsl:choose>
							<xsl:when test="$Quantity &gt; 0">
								<xsl:value-of select="$Quantity"/>
							</xsl:when>
							<xsl:when test="$Quantity &lt; 0">
								<xsl:value-of select="$Quantity * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>
					<xsl:variable name="AvgPrice">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL32"/>
						</xsl:call-template>
					</xsl:variable>
					<AvgPX>
						<xsl:choose>
							<xsl:when test="$AvgPrice &gt; 0">
								<xsl:value-of select="$AvgPrice"/>
							</xsl:when>
							<xsl:when test="$AvgPrice &lt; 0">
								<xsl:value-of select="$AvgPrice * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</AvgPX>
					<xsl:variable name="Side" select="COL40"/>
					<Side>
						<xsl:choose>
							<xsl:when test="$Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="$Side='Buy'">
										<xsl:value-of select="'Buy to Open'"/>
									</xsl:when>
									<xsl:when test="$Side='Sell'">
										<xsl:value-of select="'Sell to Close'"/>
									</xsl:when>
									<xsl:when test="$Side='Sell short'">
										<xsl:value-of select="'Sell to Open'"/>
									</xsl:when>
									<xsl:when test="$Side='BUY TO CLOSE'">
										<xsl:value-of select="'Buy to Close'"/>
									</xsl:when>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$Side='BUY'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$Side='SELL'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>
									<xsl:when test="$Side='SELL SHORT'">
										<xsl:value-of select="'Sell Short'"/>
									</xsl:when>
									<xsl:when test="$Side='BUY TO CLOSE'">
										<xsl:value-of select="'Buy to Close'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</Side>
					<xsl:variable name="varStampDuty">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="translate(COL55,'$','')"/>
						</xsl:call-template>
					</xsl:variable>
					<SecFee>
						<xsl:choose>
							<xsl:when test ="number($varStampDuty) and $varStampDuty &gt; 0">
								<xsl:value-of select ="$varStampDuty"/>
							</xsl:when>
							<xsl:when test ="number($varStampDuty) and $varStampDuty &lt; 0">
								<xsl:value-of select ="$varStampDuty * -1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecFee>

					<xsl:variable name="PB_BROKER_NAME">
						<xsl:value-of select="COL62"/>
					</xsl:variable>
					<xsl:variable name="PRANA_BROKER_ID">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
					</xsl:variable>
					<CounterPartyID>
						<xsl:choose>
							<xsl:when test="number($PRANA_BROKER_ID)">
								<xsl:value-of select="$PRANA_BROKER_ID"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CounterPartyID>

					<xsl:variable name="Fees">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL35"/>
						</xsl:call-template>
					</xsl:variable>

					<OtherBrokerFees>
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
					</OtherBrokerFees>

					<xsl:variable name="Date">
						<xsl:value-of select ="concat(substring(COL27,5,2),'/',substring(COL27,7,2),'/',substring(COL27,1,4))"/>
					</xsl:variable>

					<TradeDate>
						<xsl:value-of select="$Date"/>
					</TradeDate>

					<xsl:variable name="Date1">
						<xsl:value-of select ="concat(substring(COL29,5,2),'/',substring(COL29,7,2),'/',substring(COL29,1,4))"/>
					</xsl:variable>

					<SettlementDate>
						<xsl:value-of select="$Date1"/>
					</SettlementDate>

					<xsl:variable name="Commission">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL36"/>
						</xsl:call-template>
					</xsl:variable>

					<Commission>
						<xsl:choose>
							<xsl:when test="$Commission &gt; 0">
								<xsl:value-of select="$Commission"/>

							</xsl:when>
							<xsl:when test="$Commission &lt; 0">
								<xsl:value-of select="$Commission * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</Commission>

					<PBSymbol>
						<xsl:value-of select="$PB_COMPANY_NAME"/>
					</PBSymbol>
					<xsl:variable name="NetNotionalValue">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL38"/>
						</xsl:call-template>
					</xsl:variable>

					<NetNotionalValue>
						<xsl:choose>
							<xsl:when test="$NetNotionalValue &gt; 0">
								<xsl:value-of select="$NetNotionalValue"/>

							</xsl:when>
							<xsl:when test="$NetNotionalValue &lt; 0">
								<xsl:value-of select="$NetNotionalValue * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</NetNotionalValue>

					<xsl:variable name="NetNotionalValueBase">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL59"/>
						</xsl:call-template>
					</xsl:variable>

					<NetNotionalValueBase>
						<xsl:choose>
							<xsl:when test="$NetNotionalValueBase &gt; 0">
								<xsl:value-of select="$NetNotionalValueBase"/>

							</xsl:when>
							<xsl:when test="$NetNotionalValueBase &lt; 0">
								<xsl:value-of select="$NetNotionalValueBase * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</NetNotionalValueBase>

					<CounterParty>
						<xsl:choose>
							<xsl:when test="COL67='Jones Trading'">
								<xsl:value-of select="'JONE'"/>
							</xsl:when>
							

							<xsl:otherwise>
								<xsl:value-of select="COL67"/>
							</xsl:otherwise>

						</xsl:choose>
					</CounterParty>

					<SMRequest>
						<xsl:value-of select="'true'"/>
					</SMRequest>
				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>