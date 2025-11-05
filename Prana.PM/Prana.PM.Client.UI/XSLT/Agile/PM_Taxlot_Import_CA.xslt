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




	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="(COL5)"/>
					</xsl:call-template>
				</xsl:variable>

				<!--<xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="(COL7 * 3)"/>
          </xsl:call-template>
        </xsl:variable>-->

				<xsl:if test="number($Position) and (COL7='Long'or COL7='Short')">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Windcrest'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" select="COL3"/>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="contains(COL16,'Option') ">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>

						</Symbol>
						<IDCOOptionSymbol>
              
							
							<xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

				  <xsl:when test="contains(COL16,'Option') ">
					  <xsl:value-of select="concat(substring(COL5,1,21),'U')"/>
                </xsl:when>

								<xsl:when test="contains(COL16,'Equity')">
									<xsl:value-of select="''"/>
								</xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>

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

						<xsl:variable name="Side" select="COL7"/>


						<SideTagValue>
							<xsl:choose>
								<xsl:when test="contains(COL3,'O:')">
									<xsl:choose>
										<xsl:when test="$Side='Long'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$Side='Short'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:when test="$Side='Cover Short'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='Sale'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Side='Long'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$Side='Sale'">
											<xsl:value-of select="'2'"/>
										</xsl:when>

										<xsl:when test="$Side='Cover Short'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='Short'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
						

							</xsl:choose>
						</SideTagValue>


						<xsl:variable name="COL5">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>




						<!--<xsl:variable name="CostBasis">
							<xsl:choose>
								<xsl:when test="COL2 = 'Option'">
									<xsl:value-of select="$COL12 div ($Position * 100)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$COL12 div $Position"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


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
						</CostBasis>-->
						<xsl:variable name="Cost">
						

								<!--<xsl:when test="contains(COL3,'O:') ">-->
									<xsl:choose>
										<xsl:when test="contains(COL3,'O:') ">
											<xsl:value-of select="(COL11 div $Position) div 100"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="COL11 div COL5"/>
										</xsl:otherwise>
									</xsl:choose>
								<!--</xsl:when>-->



								<!--<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="number(COL5)">
											<xsl:value-of select="COL10"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>-->
							


							
						</xsl:variable>



						<xsl:choose>
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
						</xsl:choose>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>
						
						
						
						
						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL1100"/>
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

						<xsl:variable name="Fees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL200"/>
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






						<xsl:variable name="FXRate" >
							<xsl:choose>

								<xsl:when test="COL9='GBP'or COL9='NZD' or COL9='EUR' or COL9='AUD'">
									<xsl:value-of select="translate(COL12,',','') div translate(COL11,',','')"/>
								</xsl:when>

								<xsl:when test="COL9='USD'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="translate(COL12,',','') div translate(COL11,',','')"/>
								</xsl:otherwise>
								
							</xsl:choose>
						</xsl:variable>

						<FXRate>
							<xsl:choose>

								<xsl:when test ="number($FXRate) ">
									<xsl:value-of select ="$FXRate"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</FXRate>


						<xsl:variable name="Year1" select="substring-after(substring-after(COL7,'/'),'/')"/>
            <xsl:variable name="Month" select="substring-before(substring-after(COL7,'/'),'/')"/>
            <xsl:variable name="Day" select="substring-before(COL7,'/')"/>

						<CounterPartyID>
							<xsl:value-of select="109"/>
						</CounterPartyID>
					
			
						

						<PositionStartDate>
           
              <xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>
            </PositionStartDate>

						<OriginalPurchaseDate>
							<xsl:value-of select="COL2"/>
						</OriginalPurchaseDate>

						
						

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>