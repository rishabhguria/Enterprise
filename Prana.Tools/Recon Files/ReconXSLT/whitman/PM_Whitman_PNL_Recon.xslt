<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

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
			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="translate(COL41,',','')"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Cash) and normalize-space(COL9)='KID SHELLEEN LLC'">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Grays'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL13"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<!--<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL12,'FU')">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>-->


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL17!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<!--<xsl:when test="$Symbol!=''">
                                  <xsl:value-of select="$Symbol"/>
                                    </xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<Bloomberg>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL17!='*'">
									<xsl:value-of select="COL17"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Bloomberg>

						
						<xsl:variable name="PB_FUND_NAME" select="COL5"/>
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
						<Asset>
							<xsl:value-of select="COL12"/>
						</Asset>
						<EndingQuantity>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</EndingQuantity>
						<!--<TotalRealizedPnl>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalRealizedPnl>-->
						<!--<CashValueLocal>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueLocal>-->
						<TradeDate>
							<xsl:value-of select="COL10"/>
						</TradeDate>-->
						<Currency>
							<xsl:value-of select="'USD'"/>
						</Currency>
						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>