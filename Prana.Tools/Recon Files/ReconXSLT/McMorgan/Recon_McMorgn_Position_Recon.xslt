<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''),'$',''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				
				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL22) div 100000"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varQuantity) ">

					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Fidelity'"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL19)"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
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
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
					
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL5)"/>
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
						
						<xsl:variable name = "varSide" >
							<xsl:value-of select="normalize-space(COL23)"/>
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="contains($varSide,'+')">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="contains($varSide,'-')">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="varMarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL20) div 1000000000"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="$varMarkPrice &lt;0">
									<xsl:value-of select ="$varMarkPrice*(-1)"/>
								</xsl:when>
								<xsl:when test ="$varMarkPrice &gt;0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="varMarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL77) div 100"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test ="$varMarketValue &lt;0">
									<xsl:value-of select ="$varMarketValue*(-1)"/>
								</xsl:when>
								<xsl:when test ="$varMarketValue &gt;0">
									<xsl:value-of select ="$varMarketValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>