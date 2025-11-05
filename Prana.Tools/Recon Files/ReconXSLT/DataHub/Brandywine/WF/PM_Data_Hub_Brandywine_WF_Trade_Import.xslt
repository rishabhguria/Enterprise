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


	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month='January'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month='February'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month='March'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month='April'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month='May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month='June'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month='July'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month=August">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month='September'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month='October'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month='November'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month='December'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">

		<DocumentElement>


			<xsl:for-each select ="//PositionMaster">
				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL63"/>
					</xsl:call-template>
				</xsl:variable>

        <xsl:variable name="varAssetSpot">
          <xsl:value-of select="substring(COL16,1,4)"/>
        </xsl:variable>

				<xsl:choose>
				<xsl:when test="number($Position) and (COL62='SALE' or COL62='PURCHASE') and not(contains(COL30,'CASH EQUIVALENTS'))and COL62!='DIVIDEND' and not(contains(COL19,'STATE STREET BANK &amp; TRUST CO.'))">
					<!--<xsl:when test="number($Position) and contains(COL19,'CURRENCY ON ACCOUNT')">-->
						<PositionMaster>

							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'Wells Fargo'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:choose>
									<xsl:when test="(COL40='' or COL40='*') and COL25!='FORWARD CONTRACTS'">
										<xsl:value-of select="normalize-space(COL15)"/>									
									</xsl:when>
									<xsl:when test="COL25='FORWARD CONTRACTS'">
										<xsl:value-of select="concat(normalize-space(COL15),' ',COL33)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="concat(normalize-space(COL15),':',COL40)"/>
									</xsl:otherwise>
								</xsl:choose>	
								
								
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>


							<xsl:variable name = "PB_SUFFIX_CODE" >
								<xsl:value-of select ="substring-after(COL,' ')"/>
							</xsl:variable>

							<xsl:variable name ="PRANA_SUFFIX_NAME">
								<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
							</xsl:variable>

              <xsl:variable name="Asset">
                <xsl:choose>
                  <xsl:when test="substring(COL16,1,4)='SPOT'">
                    <xsl:value-of select="'Spot'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

							<xsl:variable name="varSymbol">
								<xsl:value-of select="COL43"/>
							</xsl:variable>

							<xsl:variable name="varISIN">
								<xsl:value-of select="COL22"/>
							</xsl:variable>

							<xsl:variable name="varSEDOL">
								<xsl:value-of select="normalize-space(COL40)"/>
							</xsl:variable>


							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									<xsl:when test="$varSymbol!='*'">
										<xsl:value-of select="$varSymbol"/>
									</xsl:when>
									<xsl:when test="$varSEDOL!='*'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="$varISIN!='*'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>


							<SEDOL>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="$varSymbol!='*'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="$varSEDOL!='*'">
										<xsl:value-of select="$varSEDOL"/>
									</xsl:when>
									<xsl:when test="$varISIN!='*'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SEDOL>


							<ISIN>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="$varSymbol!='*'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="$varSEDOL!='*'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="$varISIN!='*'">
										<xsl:value-of select="$varISIN"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ISIN>

							<xsl:variable name="PB_FUND_NAME" select="COL2"/>

							<xsl:variable name ="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

							<PositionStartDate>
								<xsl:value-of select ="COL44"/>
							</PositionStartDate>


							<PositionSettlementDate>
								<xsl:value-of select ="''"/>
							</PositionSettlementDate>
							
							<xsl:variable name="varQuantity">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL21"/>
								</xsl:call-template>
							</xsl:variable>


							
							<NetPosition>
                <xsl:choose>
                  <xsl:when test="$Asset='Spot'">
                    <xsl:choose>
                      <xsl:when test="COL15='EURO SPOT' or COL15='AUSTRALIAN DOLLAR SPOT' or COL15='GREAT BRITISH POUNDS SPOT'">
                        <xsl:choose>
                          <xsl:when test ="$Position &lt;0">
                            <xsl:value-of select ="$Position*-1"/>
                          </xsl:when>
                          <xsl:when test ="$Position &gt;0">
                            <xsl:value-of select ="$Position"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select ="0"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:choose>
                          <xsl:when test ="$varQuantity &lt;0">
                            <xsl:value-of select ="$varQuantity*-1"/>
                          </xsl:when>
                          <xsl:when test ="$varQuantity &gt;0">
                            <xsl:value-of select ="$varQuantity"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select ="0"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test ="$Position &lt;0">
                        <xsl:value-of select ="$Position*-1"/>
                      </xsl:when>

                      <xsl:when test ="$Position &gt;0">
                        <xsl:value-of select ="$Position"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select ="0"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </NetPosition>

							<xsl:variable name="varSide">
								<xsl:value-of select="COL62"/>
							</xsl:variable>

							<SideTagValue>
                <xsl:choose>
                  <xsl:when test="$Asset='Spot'">
                    <xsl:choose>
                      <xsl:when test="COL15='EURO SPOT' or COL15='AUSTRALIAN DOLLAR SPOT' or COL15='GREAT BRITISH POUNDS SPOT'">
                        <xsl:choose>
                          <xsl:when test ="$varSide='PURCHASE'">
                            <xsl:value-of select ="'2'"/>
                          </xsl:when>
                          <xsl:when test ="$varSide ='SALE'">
                            <xsl:value-of select ="'1'"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select ="''"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:choose>
                          <xsl:when test ="$varSide='PURCHASE'">
                            <xsl:value-of select ="'1'"/>
                          </xsl:when>
                          <xsl:when test ="$varSide ='SALE'">
                            <xsl:value-of select ="'2'"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select ="''"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test ="$Position &gt;0">
                        <xsl:value-of select ="'1'"/>
                      </xsl:when>
                      <xsl:when test ="$Position &lt;0">
                        <xsl:value-of select ="'2'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select ="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>							
							
							</SideTagValue>



							<xsl:variable name="CostValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="$varQuantity div $Position"/>
								</xsl:call-template>
							</xsl:variable>
							<!--<xsl:variable name="CostValue1">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="substring-before(substring-after(substring-after(substring-after(COL50,' '),' '),' '),' ')"/>
								</xsl:call-template>
							</xsl:variable>-->
							<xsl:variable name="CostValue1">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL20"/>
								</xsl:call-template>
							</xsl:variable>
							<xsl:variable name="varCostValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="$Position div $varQuantity"/>
								</xsl:call-template>
							</xsl:variable>
							
				<CostBasis>
                <xsl:choose>
                  <xsl:when test="$Asset='Spot'">
                    <xsl:choose>
                      <xsl:when test="COL15='EURO SPOT' or COL15='AUSTRALIAN DOLLAR SPOT' or COL15='GREAT BRITISH POUNDS SPOT'">
                        <xsl:choose>
                          <xsl:when test ="$CostValue &lt;0">
                            <xsl:value-of select ="$CostValue*-1"/>
                          </xsl:when>
                          <xsl:when test ="$CostValue &gt;0">
                            <xsl:value-of select ="$CostValue"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select ="0"/>
                          </xsl:otherwise>
                        </xsl:choose>
						  
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:choose>
                          <xsl:when test ="$varCostValue &lt;0">
                            <xsl:value-of select ="$varCostValue*-1"/>
                          </xsl:when>
                          <xsl:when test ="$varCostValue &gt;0">
                            <xsl:value-of select ="$varCostValue"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select ="0"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
					<xsl:when test="contains(COL33,'FOREIGN EQUITIES')">
						<xsl:value-of select ="substring-before(substring-after(COL50,'SHARES AT '),' ')"/>
					</xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test ="$CostValue1 &lt;0">
                        <xsl:value-of select ="$CostValue1*-1"/>
                      </xsl:when>
                      <xsl:when test ="$CostValue1 &gt;0">
                        <xsl:value-of select ="$CostValue1"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select ="0"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </CostBasis>

							<xsl:variable name="FXRate">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL20"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varFXRate">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="$FXRate div $CostValue"/>
								</xsl:call-template>
							</xsl:variable>


							<xsl:variable name="varCommissionAndFees">
								<xsl:choose>
									<xsl:when test="contains(COL50,'FEES')">
										<xsl:value-of select="substring-before(substring-after(substring-after(COL50,' '),' '),' ')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="substring-after(COL50,'$')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<xsl:variable name="varCommission">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="$varCommissionAndFees"/>
								</xsl:call-template>
							</xsl:variable>
							
							<!--<xsl:variable name="Commission">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="$varCommission div $varFXRate "/>
								</xsl:call-template>
							</xsl:variable>-->

							<xsl:variable name="Commission">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL18"/>
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


							<xsl:variable name="varSecFee">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL38"/>
								</xsl:call-template>
							</xsl:variable>
							<SecFee>

								<xsl:choose>
									<xsl:when test="$varSecFee &gt; 0">
										<xsl:value-of select="$varSecFee"/>
									</xsl:when>
									<xsl:when test="$varSecFee &lt; 0">
										<xsl:value-of select="$varSecFee * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>


								</xsl:choose>
							</SecFee>


							<xsl:variable name="varFXRateSpot">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="$varQuantity div $Position"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varCurrency">
								<xsl:value-of select="substring-before(substring-before(substring-after(COL50,'SHARES AT '),' '),' ')"/>
							</xsl:variable>
							<xsl:variable name="varPriceNonUSD">
								<xsl:value-of select="substring-before(substring-after(COL50,'SHARES AT '),' ')"/>
							</xsl:variable>
							<xsl:variable name="varFXRateNonUSD">
								<xsl:choose>
									<xsl:when test="$varCurrency='EUR' or $varCurrency='AUD' or $varCurrency='NZD' or $varCurrency='GBP'">
										<xsl:value-of select="COL20 div $varPriceNonUSD"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL20 * $varPriceNonUSD"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
              <FXRate>
                <xsl:choose>
                  <xsl:when test="$Asset='Spot'">
                    <xsl:choose>
                      <xsl:when test="COL15='EURO SPOT' or COL15='AUSTRALIAN DOLLAR SPOT' or COL15='GREAT BRITISH POUNDS SPOT'">
                        <xsl:value-of select="1"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:choose>
                          <xsl:when test ="$varFXRateSpot &lt;0">
                            <xsl:value-of select ="$varFXRateSpot*-1"/>
                          </xsl:when>
                          <xsl:when test ="$varFXRateSpot &gt;0">
                            <xsl:value-of select ="$varFXRateSpot"/>
                          </xsl:when>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
					<xsl:when test="contains(COL33,'FOREIGN EQUITIES')">
						<xsl:value-of select ="$varFXRateNonUSD"/>
					</xsl:when>
                  <xsl:otherwise>
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
                  </xsl:otherwise>
                </xsl:choose>

              </FXRate>

							<PBSymbol>
								<xsl:value-of select ="$PB_SYMBOL_NAME"/>
							</PBSymbol>


						</PositionMaster>
					</xsl:when>
					
				</xsl:choose>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
