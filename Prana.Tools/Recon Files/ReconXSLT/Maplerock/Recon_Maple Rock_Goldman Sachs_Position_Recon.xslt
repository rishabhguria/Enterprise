<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test="COL7 != 'Trade Date Quantity' and COL7 != 0 and not(contains(COL4,' CFD OTC'))">
					<PositionMaster>
						
						<!--FUNDNAME SECTION-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='MapleRock']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>



						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>


						
								
								<Quantity>
									<xsl:value-of select="COL7"/>
								</Quantity>
							
								<Side/>
									
								
								

						<xsl:choose>
							<xsl:when test ="boolean(number(COL10))">
								<MarkPrice>
									<xsl:value-of select="COL10"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>

	           <MarketValue>
							<xsl:value-of select="COL13"/>
						</MarketValue>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="number(COL14)">
									<xsl:value-of select="COL14"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>
						
<!-- SYMBOL, PBSYMBOL, COMPANYNAME SECTION -->


            <!--<xsl:variable name="OptionUnderlyingSymbol">
              <xsl:choose>
                <xsl:when test="starts-with(COL5,'$')">
                  <xsl:variable name ="VarTest" select ="normalize-space(substring-after(COL4,'/'))"/>
                  <xsl:variable name ="Varrt" select ="normalize-space(substring-before($VarTest,'('))"/>
                  <xsl:value-of select ="$Varrt"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="OptionMonth">
              <xsl:choose>
                <xsl:when test="starts-with(COL5,'$')">
                  <xsl:variable name ="varLength" select ="string-length(COL5)"/>
                  <xsl:value-of select ="substring(COL5,$varLength - 1,1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Strike">
              <xsl:choose>
                <xsl:when test="starts-with(COL5,'$')">
                  <xsl:variable name ="varafter" select ="substring-after(COL4,'@')"/>
                  <xsl:variable name ="varStr" select ="normalize-space(substring-before($varafter,'EXP'))"/>
                  <xsl:variable name ="varStrikeDecimal" select ="substring-after($varStr,'.')"/>
                  <xsl:variable name ="varStrikeInt" select ="substring-before($varStr,'.')"/>
                  <xsl:choose>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 1">
                      <xsl:value-of select ="concat($varStr,'0')"/>
                    </xsl:when>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 2">
                      <xsl:value-of select ="$varStr"/>
                    </xsl:when>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) &gt; 2">
                      <xsl:value-of select ="concat($varStrikeInt,'.',substring($varStrikeDecimal,1,2))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select ="concat($varStr,'.00')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="ExpYear">
              <xsl:choose>
                <xsl:when test="starts-with(COL5,'$')">
                  <xsl:variable name ="varafter" select ="substring-after(COL4,'EXP')"/>
                  <xsl:value-of select ="substring($varafter,10,2)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->
            
            
						<xsl:variable name="PB_COMPANY_NAME" select="COL4"/>

						<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='MapleRock']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>
						
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select='$PRANA_SYMBOL_NAME'/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select ="''"/>
								</IDCOOptionSymbol>
								<SEDOL>
									<xsl:value-of select ="''"/>
								</SEDOL>
							</xsl:when>
							<!--<xsl:when test="starts-with(COL5,'$')">-->
							<xsl:when test="string-length(COL5) &gt; 20">
								<!--<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL5)"/>
								</xsl:variable>-->
								<!--<Symbol>
									  --><!--<xsl:value-of select="concat(substring(COL5,2,($varLength - 3)),' ',substring(COL5,($varLength - 1),$varLength))"/>--><!--
									  --><!--<xsl:value-of select ="concat('O:',$OptionUnderlyingSymbol,' ',$ExpYear,$OptionMonth,$Strike)"/>--><!--
									<xsl:value-of select ="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select ="concat(COL17,'U')"/>
								</IDCOOptionSymbol>-->
								<Symbol>
									<xsl:choose>
										<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
											<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</Symbol>

								<IDCOOptionSymbol>
									<xsl:choose>
										<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
											<xsl:value-of select ="''"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat(COL18,'U')"/>
										</xsl:otherwise>
									</xsl:choose>
								</IDCOOptionSymbol>
								<SEDOL>
									<xsl:value-of select ="''"/>
								</SEDOL>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									
								<xsl:when test ="COL9='USD'">
									<Symbol>
										<xsl:value-of select='COL5'/>
									</Symbol>
									<IDCOOptionSymbol>
										<xsl:value-of select ="''"/>
									</IDCOOptionSymbol>
									<SEDOL>
										<xsl:value-of select ="''"/>
									</SEDOL>
								</xsl:when>
									<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select ="''"/>
								</IDCOOptionSymbol>
										<SEDOL>
											<xsl:value-of select ="COL5"/>
										</SEDOL>
									</xsl:otherwise>

								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>

						<!--<ISINSymbol>
							<xsl:value-of select="normalize-space(COL14)"/>
						</ISINSymbol>

						<SEDOLSymbol>
							<xsl:value-of select="normalize-space(COL15)"/>
						</SEDOLSymbol>

						<CUSIP>
							<xsl:value-of select="normalize-space(COL16)"/>
						</CUSIP>-->											
						
						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>

						

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
