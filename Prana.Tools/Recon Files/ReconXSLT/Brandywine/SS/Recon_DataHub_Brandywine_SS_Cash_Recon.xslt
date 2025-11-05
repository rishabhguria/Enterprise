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
						<xsl:with-param name="Number" select="substring(COL1,27,16)"/>
					</xsl:call-template>
				</xsl:variable>


				<xsl:if test="number($Cash)">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'SS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="PB_FUND_NAME" select="normalize-space(substring(COL1,9,12))"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>


						<Currency>
							<xsl:value-of select="substring(COL1,24,3)"/>
						</Currency>


						<CashValueLocal>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueLocal>
						
					

						<xsl:variable name="CashValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,81,16)"/>
							</xsl:call-template>
						</xsl:variable>

						<CashValueBase>
							<xsl:choose>
								<xsl:when test="number($CashValueBase)">
									<xsl:value-of select="$CashValueBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueBase>

						<xsl:variable name="varYear">
							<xsl:value-of select="substring(COL1,1,4)"/>
						</xsl:variable>
						<xsl:variable name="varMonth">
							<xsl:value-of select="substring(COL1,5,2)"/>
						</xsl:variable>
						<xsl:variable name="varDay">
							<xsl:value-of select="substring(COL1,7,2)"/>
						</xsl:variable>
						<TradeDate>
							<xsl:value-of select ="concat($varMonth,'/',$varDay,'/',$varYear)"/>
						</TradeDate>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>