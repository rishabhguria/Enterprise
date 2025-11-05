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
	
	<xsl:template name="FormatDate">
        <xsl:param name="DateTime"/>
        <!--  converts date time double number to 18/12/2009  -->
        <xsl:variable name="l">
        <xsl:value-of select="$DateTime + 68569 + 2415019"/>
        </xsl:variable>
        <xsl:variable name="n">
        <xsl:value-of select="floor(((4 * $l) div 146097))"/>
        </xsl:variable>
        <xsl:variable name="ll">
        <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))"/>
        </xsl:variable>
        <xsl:variable name="i">
        <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))"/>
        </xsl:variable>
        <xsl:variable name="lll">
        <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31"/>
        </xsl:variable>
        <xsl:variable name="j">
        <xsl:value-of select="floor(((80 * $lll) div 2447))"/>
        </xsl:variable>
        <xsl:variable name="nDay">
        <xsl:value-of select="$lll - floor(((2447 * $j) div 80))"/>
        </xsl:variable>
        <xsl:variable name="llll">
        <xsl:value-of select="floor(($j div 11))"/>
        </xsl:variable>
        <xsl:variable name="nMonth">
        <xsl:value-of select="floor($j + 2 - (12 * $llll))"/>
        </xsl:variable>
        <xsl:variable name="nYear">
        <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)"/>
        </xsl:variable>
        <xsl:variable name="varMonthUpdated">
        <xsl:choose>
        <xsl:when test="string-length($nMonth) = 1">
        <xsl:value-of select="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
        <xsl:value-of select="$nMonth"/>
        </xsl:otherwise>
        </xsl:choose>
        </xsl:variable>
        <xsl:variable name="nDayUpdated">
        <xsl:choose>
        <xsl:when test="string-length($nDay) = 1">
        <xsl:value-of select="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
        <xsl:value-of select="$nDay"/>
        </xsl:otherwise>
        </xsl:choose>
        </xsl:variable>
        <xsl:value-of select="$varMonthUpdated"/>
        <xsl:value-of select="'/'"/>
        <xsl:value-of select="$nDayUpdated"/>
        <xsl:value-of select="'/'"/>
        <xsl:value-of select="$nYear"/>
        </xsl:template>

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

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL20)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="'Ibew Pacific Coast 6746022837'"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME" />
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>						
						
						<SideTagValue>							
							<xsl:choose>
								<xsl:when test="contains(normalize-space(COL37),'10')">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL37),'20')">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>								
						</SideTagValue>
						
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>							
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL21)"/>
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
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL24)"/>
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

						<xsl:variable name="SecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL26)"/>
							</xsl:call-template>
						</xsl:variable>

						<SecFee>
							<xsl:choose>
								<xsl:when test="$SecFee &gt; 0">
									<xsl:value-of select="$SecFee"/>
								</xsl:when>
								<xsl:when test="$SecFee &lt; 0">
									<xsl:value-of select="$SecFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>
							
						<NetPosition>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>			
						
						<xsl:variable name="varPositionStartDate">
                            <xsl:call-template name="FormatDate">
                               <xsl:with-param name="DateTime" select="normalize-space(COL2)"/>
                            </xsl:call-template>				
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select="$varPositionStartDate"/>
						</PositionStartDate>
						
						<xsl:variable name="varPositionSettlementDate">
                           <xsl:call-template name="FormatDate">
                               <xsl:with-param name="DateTime" select="normalize-space(COL3)"/>
                            </xsl:call-template>								
						</xsl:variable>

						<PositionSettlementDate>
							<xsl:value-of select="$varPositionSettlementDate"/>
						</PositionSettlementDate>									
						
						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL54)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/CounterPartyMapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@MappedBrokerCode=$PB_BROKER_NAME]/@BrokerCode"/>
						</xsl:variable>
						
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID!='')">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


