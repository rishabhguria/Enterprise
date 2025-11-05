<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL4"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($NetPosition)">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'WH'"/>
						</xsl:variable>
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="Symbol">
							<xsl:value-of select="COL2"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
					<xsl:variable name="PB_FUND_NAME" select="COL6"/>
			 		<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<FundName>
							<xsl:choose>
						 	<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</FundName>
						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL9)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_BROKER_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/BrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBroker"/>
						</xsl:variable>
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="$PRANA_BROKER_NAME!=''">
									<xsl:value-of select="$PRANA_BROKER_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_BROKER_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>
						<NetPosition>
							<xsl:choose>
								<xsl:when test="$NetPosition &gt; 0">
									<xsl:value-of select="$NetPosition"/>
								</xsl:when>
								<xsl:when test="$NetPosition &lt; 0">
									<xsl:value-of select="$NetPosition* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>
						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>
								</xsl:when>
								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>
						<xsl:variable name="Commission">
							<xsl:value-of select="COL7"/>
						</xsl:variable>
						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>
						<xsl:variable name="Side" select="normalize-space(COL3)"/>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Side='BUY'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Side='SELL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$Side='SELL SHORT'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$Side='BUY TO OPEN'">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when test="$Side='BUY TO CLOSE '">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$Side='SELL TO OPEN '">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								<xsl:when test="$Side='SELL TO CLOSE'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:when test="$Side='BUY TO COVER'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$Side='SELL TO COVER'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>
						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>
						<xsl:variable name ="Date" select="COL1"/>
						<PositionStartDate>
							<xsl:value-of select="$Date"/>
						</PositionStartDate>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>