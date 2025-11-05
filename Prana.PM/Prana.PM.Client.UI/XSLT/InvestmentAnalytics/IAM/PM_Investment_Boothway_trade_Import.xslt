<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
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
	
	<xsl:variable name="smallcase" select="'abcdefghijklmnopqrstuvwxyz'" />
	<xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />




	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="(COL10)"/>
					</xsl:call-template>
				</xsl:variable>

				<!--<xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="(COL7 * 3)"/>
          </xsl:call-template>
        </xsl:variable>-->

				<xsl:if test="number($Position)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Windcrest'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL11)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:variable name="Symbol" select="translate(COL5, $smallcase, $uppercase)"/>-->
						

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<!--<xsl:when test="contains(COL46,'Option') ">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>-->

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>

						</Symbol>
						<!--<IDCOOptionSymbol>
              
							
							<xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

				  <xsl:when test="string-length(COL5 &gt; 10) ">
					  <xsl:value-of select="concat(substring(COL5,1,21),'U')"/>
                </xsl:when>

								--><!--<xsl:when test="contains(COL46,'Equity')">
									<xsl:value-of select="''"/>
								</xsl:when>--><!--

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>-->

						<xsl:variable name="PB_FUND_NAME" select="''"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name="Side" select="COL8"/>


						<SideTagValue>
							<xsl:choose>
								<!--<xsl:when test="$Side='BY'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Side='SS'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$Side='SL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$Side='CS'">
									<xsl:value-of select="'B'"/>
								</xsl:when>-->
								<!--<xsl:when test="CS">
									<xsl:value-of select="'B'"/>
								</xsl:when>-->
								<xsl:when test="($Position &gt; 0)">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when test="($Position &lt; 0)">
									<xsl:value-of select="'2'"/>
								</xsl:when>

								<!--<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'5'"/>
								</xsl:when>-->
								
								

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</SideTagValue>


						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL19"/>
							</xsl:call-template>
						</xsl:variable>




						<!--<xsl:variable name="CostBasis">
							<xsl:choose>
								<xsl:when test="COL5 = 'Option'">
									<xsl:value-of select="$COL42 div ($Position * 100)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$COL42 div $Position"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->


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
						<!--<xsl:variable name="Cost">
							<xsl:choose>
								<xsl:when test="number(COL4) and COL3='COMMON STOCK'">
									<xsl:value-of select="COL12 div COL4"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL12 div ( COL4 * 100)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->



						<!--<xsl:choose>
							<xsl:when test ="boolean(number($Cost))">
								<CostBasis>
									<xsl:value-of select="$Cost"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>-->

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>
						
						
						
						
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

						<!--<xsl:variable name="Fees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>

						<Fees>

							<xsl:choose>

								<xsl:when test="$Fees &gt; 0">
									<xsl:value-of select="$Fees"/>
								</xsl:when>

								<xsl:when test="$Fees &lt; 0">
									<xsl:value-of select="$Fees * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Fees>

						<xsl:variable name="SecFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>

						<SecFees>

							<xsl:choose>

								<xsl:when test="$SecFees &gt; 0">
									<xsl:value-of select="$SecFees"/>
								</xsl:when>

								<xsl:when test="$SecFees &lt; 0">
									<xsl:value-of select="$SecFees * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</SecFees>-->





						<!--<xsl:variable name="FXRate" select="COL21"/>

						<FXRate>
							<xsl:choose>

								<xsl:when test ="number($FXRate) ">
									<xsl:value-of select =" $FXRate "/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</FXRate>-->


						<!--<xsl:variable name="Year1" select="substring($Date,1,4)"/>
            <xsl:variable name="Month" select="substring($Date,5,2)"/>
            <xsl:variable name="Day" select="substring($Date,7,2)"/>--><!--

						--><!--<xsl:variable name="PB_CountnerParty" select="normalize-space(COL16)"/>
						<xsl:variable name="PRANA_CounterPartyID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'CON']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_CounterPartyID)">
									<xsl:value-of select="$PRANA_CounterPartyID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="79"/>
								</xsl:otherwise>
							</xsl:choose>--><!--
						<--><!--/CounterPartyID>--><!--

						--><!--<CounterPartyID>
							<xsl:value-of select="'0'"/>
						</CounterPartyID>-->
					
			
						

						<PositionStartDate>           
                        <xsl:value-of select="COL1"/>
                       </PositionStartDate>

						<!--<OriginalPurchaseDate>
							<xsl:value-of select="COL4"/>
						</OriginalPurchaseDate>-->

						
						

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>


</xsl:stylesheet>