<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
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

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL13)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL23)"/>
						</xsl:variable>

						<xsl:variable name="varSettlCurrency">
							<xsl:value-of select ="normalize-space(COL15)"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">							
							<xsl:choose>
								<xsl:when test="$varSettlCurrency='USD'">
									<xsl:value-of select="normalize-space(COL24)"/>
								</xsl:when>															
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:choose>
								<xsl:when test="$varSettlCurrency !='USD'">
									<xsl:value-of select="normalize-space(COL27)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>								
								<xsl:when test="$varSymbol !=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<SEDOL>
							<xsl:choose>								
								<xsl:when test="$varSEDOL!=''">
									<xsl:value-of select="$varSEDOL"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL3)"/>
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

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$varQuantity &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="$varQuantity &lt; 0">
									<xsl:value-of select="$varQuantity * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varAvgPrice">
							<xsl:choose>
								<xsl:when test="$varQuantity != 0">
									<xsl:call-template name="Translate">
										<xsl:with-param name="Number" select="normalize-space(COL8) div $varQuantity"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>									
										<xsl:value-of select="0"/>									
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varQuantity &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varQuantity &lt; 0">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<CostBasis>
							<xsl:choose>
								<xsl:when test="$varAvgPrice &gt; 0">
									<xsl:value-of select="$varAvgPrice"/>
								</xsl:when>
								<xsl:when test="$varAvgPrice &lt; 0">
									<xsl:value-of select="$varAvgPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varFXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL14)"/>
							</xsl:call-template>
						</xsl:variable>

						<FXRate>
							<xsl:choose>
								<xsl:when test="$varFXRate &gt; 0">
									<xsl:value-of select="$varFXRate"/>
								</xsl:when>
								<xsl:when test="$varFXRate &lt; 0">
									<xsl:value-of select="$varFXRate * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>

						<xsl:variable name="varPositionStartDate">
							<xsl:value-of select ="normalize-space(COL4)"/>
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select ="$varPositionStartDate"/>
						</PositionStartDate>

						<xsl:variable name="varSettlementDate">
							<xsl:value-of select ="''"/>
						</xsl:variable>

						<PositionSettlementDate>
							<xsl:value-of select ="$varSettlementDate"/>
						</PositionSettlementDate>
					

						<SettlCurrencyName>
							<xsl:value-of select ="$varSettlCurrency"/>
						</SettlCurrencyName>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>