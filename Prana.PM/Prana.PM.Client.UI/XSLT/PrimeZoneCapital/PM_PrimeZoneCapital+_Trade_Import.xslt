<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	

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
			  <xsl:for-each select="//PositionMaster">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL12"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
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


						<xsl:variable name="Symbol">
							<xsl:value-of select="normalize-space(COL8)"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL != ''">
									<xsl:value-of select ="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<ISIN>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL != ''">
									<xsl:value-of select ="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test="$Symbol!='' or $Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY"/>
								</xsl:otherwise>
							</xsl:choose>
						</ISIN>


						<NetPosition>
							<xsl:choose>
								<xsl:when test="$Quantity &gt; 0">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								<xsl:when test="$Quantity &lt; 0">
									<xsl:value-of select="$Quantity * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
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
						</CostBasis>


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL21"/>
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


						<xsl:variable name="varDate">
							<xsl:value-of select="COL16"/>
						</xsl:variable>

						<!-- <xsl:variable name="varMonth"> -->
						<!-- <xsl:value-of select="substring-before(substring-after(COL6,'/'),'/')"/> -->

						<!-- </xsl:variable> -->


						<!-- <xsl:variable name="varDay"> -->
						<!-- <xsl:value-of select="substring-after(substring-after(COL6,'/'),'/')"/> -->
						<!-- </xsl:variable> -->


						<!-- <xsl:variable name="varYear"> -->
						<!-- <xsl:value-of select="substring-before(COL6,'/')"/> -->
						<!-- </xsl:variable> -->

						<PositionStartDate>
							<!-- <xsl:value-of select ="concat($varYear,'/',$varMonth,'/',$varDay)"/>  -->
							<xsl:value-of select="substring-before(COL16,' ')"/>
						</PositionStartDate>

						<!-- <xsl:variable name="varMonth1"> -->
						<!-- <xsl:value-of select="substring-after(substring-after(COL7,'/'),'/')"/> -->

						<!-- </xsl:variable> -->


						<!-- <xsl:variable name="varDay1"> -->
						<!-- <xsl:value-of select="substring-before(substring-after(COL7,'/'),'/')"/> -->
						<!-- </xsl:variable> -->


						<!-- <xsl:variable name="varYear1"> -->
						<!-- <xsl:value-of select="substring-before(COL7,'/')"/> -->
						<!-- </xsl:variable> -->

						<xsl:variable name="varSettlementDate">
							<xsl:value-of select="substring-before(COL17,' ')"/>
						</xsl:variable>
						<PositionSettlementDate>
							<!-- <xsl:value-of select ="concat($varYear1,'/',$varMonth1,'/',$varDay1)"/> -->
							<xsl:value-of select ="$varSettlementDate"/>

						</PositionSettlementDate>

				
						<Side>
							<xsl:choose>
								<xsl:when test="$Quantity &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Quantity &lt; 0">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>



						<PBSymbol>
							<xsl:value-of select="$PB_COMPANY"/>
						</PBSymbol>

					
				</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


