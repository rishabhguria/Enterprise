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
						<xsl:with-param name="Number" select="(COL7)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) and not(contains(COL3,'MORGAN STANLEY')) ">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Windcrest'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="substring-before(COL3,' ')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" select="substring-before(COL3,' ')"/>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<!--<xsl:when test="contains(COL76,'Option') ">
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
						<IDCOOptionSymbol>
              
							
							<xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

				  <xsl:when test="string-length(COL2 &gt; 10) ">
					  <xsl:value-of select="concat(substring(COL2,1,21),'U')"/>
                </xsl:when>

								<!--<xsl:when test="contains(COL76,'Equity')">
									<xsl:value-of select="''"/>
								</xsl:when>-->

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

						<xsl:variable name="Side" select="COL6"/>


						<SideTagValue>
							<xsl:choose>
								<!--<xsl:when test="$Side='Buy' and contains(COL6,'US 15')">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when test="$Side='Cover Short' and contains(COL6,'US 15')">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$Side='Sale' and contains(COL6,'US 15')">
									<xsl:value-of select="'D'"/>
								</xsl:when>

								<xsl:when test="$Side='Short Sale' and contains(COL6,'US 15')">
									<xsl:value-of select="'C'"/>
								</xsl:when>-->

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="($Position &gt; 0) and string-length(COL2 &gt; 10)">
									<xsl:value-of select="'A'"/>
								</xsl:when>

								<xsl:when test="($Position &lt; 0) and string-length(COL2 &gt; 10)">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</SideTagValue>


						<xsl:variable name="COL7">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>




						<!--<xsl:variable name="CostBasis">
							<xsl:choose>
								<xsl:when test="COL2 = 'Option'">
									<xsl:value-of select="$COL72 div ($Position * 100)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$COL72 div $Position"/>
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
							<xsl:choose>
								<xsl:when test="number(COL7)">
									<xsl:value-of select="COL10 div COL7"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
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
						
						
						
						
						<!--<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL711"/>
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
								<xsl:with-param name="Number" select="COL712"/>
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






						<xsl:variable name="FXRate" select="COL71"/>

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
            <xsl:variable name="Day" select="substring($Date,7,2)"/>-->

						<CounterPartyID>
							<xsl:value-of select="'0'"/>
						</CounterPartyID>
					
			
						

						<!--<PositionStartDate>           
                        <xsl:value-of select="COL74"/>
                       </PositionStartDate>-->

						<OriginalPurchaseDate>
							<xsl:value-of select="COL2"/>
						</OriginalPurchaseDate>

						
						

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>