<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
	<xsl:output method="xml" indent="yes" />
	<xsl:template name="Translate">
		<xsl:param name="Number" />
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="FormatDate">
		<xsl:param name="varFullDate" />
		<xsl:variable name="varYear">
			<xsl:value-of select="substring($varFullDate, string-length($varFullDate) - 3 , 4)"/>
		</xsl:variable>
		<xsl:variable name="varWithoutYear">
			<xsl:value-of select="substring($varFullDate, 1, string-length($varFullDate) - 4)"/>
		</xsl:variable>
		<xsl:variable name="varDay">
			<xsl:choose>
				<xsl:when test="$varWithoutYear &lt; 100">
					<xsl:value-of select="concat('0',substring($varWithoutYear, string-length($varWithoutYear) - 0, 1))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring($varWithoutYear, string-length($varWithoutYear) - 1, string-length($varWithoutYear))"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="varMonth">
			<xsl:choose>
				<xsl:when test="$varWithoutYear &lt; 999">
					<xsl:value-of select="concat('0',substring($varWithoutYear, 1, 1))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring($varWithoutYear, 1, 2)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
	</xsl:template>
	<xsl:template match="/">
    
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL11)"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:choose>
					
				<xsl:when test="number($Quantity) and (normalize-space(COL18) != 'Cash and Cash Equivalents') and COL25 = '          ' ">

						<PositionMaster>

							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'Northern Trust'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="normalize-space(COL22)"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<xsl:variable name="varSEDOL">
								<xsl:value-of select="substring-after(normalize-space(COL24),'S')"/>
							</xsl:variable>

							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
                    <xsl:when test="$varSEDOL !='' and $varSEDOL !='*'">
                    <xsl:value-of select="''"/>
                  </xsl:when>      
									 <xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>

              <SEDOL>
                <xsl:choose>
                  <xsl:when test="$varSEDOL !='' and $varSEDOL !='*'">
                    <xsl:value-of select="$varSEDOL"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SEDOL>

							<xsl:variable name="TradeDate" select="''"/>

							<TradeDate>
								<xsl:value-of select="$TradeDate"/>
							</TradeDate>

							<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>

							<xsl:variable name ="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

							<Quantity>
								<xsl:choose>
									<xsl:when test="number($Quantity)">
										<xsl:value-of select="$Quantity"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>

							<xsl:variable name="Side" select="''"/>

							<Side>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>

							<PBSymbol>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</PBSymbol>

							<xsl:variable name="MarketValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL4)"/>
								</xsl:call-template>
							</xsl:variable>

							<MarketValue>
								<xsl:choose>
									<xsl:when test="number($MarketValue)">
										<xsl:value-of select="$MarketValue"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValue>

							<xsl:variable name="MarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL3)"/>
								</xsl:call-template>
							</xsl:variable>

							<MarkPrice>
								<xsl:choose>
									<xsl:when test="number($MarkPrice)">
										<xsl:value-of select="$MarkPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarkPrice>

							<xsl:variable name="MarketValueBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL5)"/>
								</xsl:call-template>
							</xsl:variable>

							<MarketValueBase>
								<xsl:choose>
									<xsl:when test="number($MarketValueBase)">
										<xsl:value-of select="$MarketValueBase"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValueBase>

              <xsl:variable name="NetNotionalValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL7"/>
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

             

							<SettlCurrency>
								<xsl:value-of select="normalize-space(COL13)"/>
							</SettlCurrency>

              <SMRequest>
                <xsl:value-of select="'True'"/>
              </SMRequest>
						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>
              
              

							<Symbol>							
								<xsl:value-of select="''"/>
							</Symbol>

							<SEDOL>								
								<xsl:value-of select="''"/>									
							</SEDOL>							

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>						

							<FundName>								
									<xsl:value-of select ="''"/>									
							</FundName>

							<Quantity>								
									<xsl:value-of select="0"/>								
							</Quantity>

							<Side>							
								    <xsl:value-of select="''"/>									
							</Side>

							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>

							<MarketValue>								
									<xsl:value-of select="0"/>									
							</MarketValue>

							<MarkPrice>								
									<xsl:value-of select="0"/>
							</MarkPrice>							

							<MarketValueBase>								
									<xsl:value-of select="0"/>
							</MarketValueBase>

							<SettlCurrency>
								<xsl:value-of select="''"/>
							</SettlCurrency>

              <SMRequest>
                <xsl:value-of select="'true'"/>
              </SMRequest>


            </PositionMaster>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'" />
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />
</xsl:stylesheet>



