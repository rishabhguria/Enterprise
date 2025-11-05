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
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($varQuantity)">

					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'NAV'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

                       <xsl:variable name="varSymbol">
                         <xsl:value-of select="COL3"/>
                       </xsl:variable>
            
           
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
                               <xsl:when test="$varSymbol !='' or $varSymbol !='*'">
                                 <xsl:value-of select="$varSymbol"/>
                               </xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
            
						<xsl:variable name="PB_FUND_NAME" select ="''"/>

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

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varQuantity)">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>
           
						<Side>
							<xsl:choose>
								<xsl:when test="$varQuantity &gt; 0">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>
								<xsl:when test="$varQuantity &lt; 0">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>
				
						<xsl:variable name="varMarkPriceLocal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$varMarkPriceLocal &gt; 0">
									<xsl:value-of select="$varMarkPriceLocal"/>
								</xsl:when>
								<xsl:when test="$varMarkPriceLocal &lt; 0">
									<xsl:value-of select="$varMarkPriceLocal * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

            <xsl:variable name="varAvgPX">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="0"/>
              </xsl:call-template>
            </xsl:variable>
            <UnitCost>
              <xsl:choose>
                <xsl:when test="number($varAvgPX)">
                  <xsl:value-of select="$varAvgPX"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </UnitCost>

            <xsl:variable name="varMarketValueLocal">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL7"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValue>
              <xsl:choose>
                <xsl:when test="number($varMarketValueLocal)">
                  <xsl:value-of select="$varMarketValueLocal"/>
                </xsl:when>             
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>
						<xsl:variable name="varMarketValueLocalBASE">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="number($varMarketValueLocalBASE)">
									<xsl:value-of select="$varMarketValueLocalBASE"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>


						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>						

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>


