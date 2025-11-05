<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
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

				<xsl:variable name ="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL9"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'USBank'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
					<xsl:variable name="varSedol">
							<xsl:value-of select="normalize-space(COL7)"/>
							</xsl:variable>
						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL8)"/>
						</xsl:variable>
						<xsl:variable name="varCurrency">
							<xsl:value-of select="substring(normalize-space(COL4),1,3)"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
							    <xsl:when test="$PRANA_SYMBOL_NAME!=''">
							    	<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							    </xsl:when>
								<xsl:when test="$varSymbol !='' and $varCurrency ='USD'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:when test="$varSedol !='' and $varCurrency !='USD'">
						   	        <xsl:value-of select="''"/>
						        </xsl:when>	                               						
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>					  

						<SEDOL>
						<xsl:choose>	
							 <xsl:when test="$varSedol !='' and $varCurrency !='USD'">
						   	<xsl:value-of select="$varSedol"/>
						   </xsl:when>
							<xsl:when test="$varSymbol !='' and $varCurrency ='USD'">
								<xsl:value-of select="''"/>
							</xsl:when>
						   <xsl:otherwise>
						   	<xsl:value-of select="''"/>
						   </xsl:otherwise>
						</xsl:choose>
						</SEDOL>		

						<xsl:variable name="PB_FUND_NAME" select="COL3"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
						<xsl:variable name="varMarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL14)"/>
							</xsl:call-template>
						</xsl:variable>

							<MarkPrice>
							<xsl:choose>
								<xsl:when test="$varMarkPrice &gt; 0">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<xsl:when test="$varMarkPrice &lt; 0">
									<xsl:value-of select="$varMarkPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>
						<Quantity>
							<xsl:choose>
								<xsl:when  test="number($varQuantity)">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>



						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>
						<AvgPrice>
							<xsl:choose>
								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>

								</xsl:when>
								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPrice>


						<xsl:variable name="varMarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL24"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="$varMarketValue &gt; 0">
									<xsl:value-of select="$varMarketValue"/>

								</xsl:when>
								<xsl:when test="$varMarketValue &lt; 0">
									<xsl:value-of select="$varMarketValue * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValueBase>
						
						<xsl:variable name="varMarketValueLocal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValue>
							<xsl:choose>
								<xsl:when test="$varMarketValueLocal &gt; 0">
									<xsl:value-of select="$varMarketValueLocal"/>

								</xsl:when>
								<xsl:when test="$varMarketValueLocal &lt; 0">
									<xsl:value-of select="$varMarketValueLocal * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValue>
						
						<Side>
							<xsl:choose>
								<xsl:when  test="$varQuantity &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when  test="$varQuantity &lt; 0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>


						<Date>
							<xsl:value-of select ="''"/>
						</Date>


						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

