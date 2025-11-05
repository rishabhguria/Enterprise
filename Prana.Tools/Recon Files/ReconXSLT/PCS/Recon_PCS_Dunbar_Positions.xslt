<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="//Comparision">

				<xsl:if test ="number(COL8)!= 0 and COL8 !='Quantity'" >
					<PositionMaster>


						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate( substring(COL1,1,4) ,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>


						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>

						</xsl:choose >

						<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL4)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name = "varInstrumentType" >
							<xsl:value-of select="translate(translate(COL2, ' ' , ''),'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name ="varFutureSymbol">
							<xsl:choose>
								<xsl:when test ="$varInstrumentType='FUTURE'">
									<xsl:variable name = "varLength" >
										<xsl:value-of select="string-length(translate(translate(COL6,'&quot;',''),' ',''))"/>
									</xsl:variable>
									<xsl:variable name = "varAfter" >
										<xsl:value-of select="substring(COL6,($varLength)-1,2)"/>
									</xsl:variable>
									<xsl:variable name = "varBefore" >
										<xsl:value-of select="substring(COL6,1,($varLength)-2)"/>
									</xsl:variable>

									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Description>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</Description>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL3,'&quot;','')"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='EQUITY'">
								<Symbol>
									<xsl:value-of select="translate(COL3,'&quot;','')"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL3,'&quot;','')"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='OPTION'">
								<Symbol>
									<!--<xsl:value-of select="translate(COL5,'&quot;','')"/>-->
									<!--<xsl:value-of select ="concat('O:',$OptionUnderlyingSymbol,' ',$ExpYear,$OptionMonth,$Strike)"/>-->
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="COL17"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="concat(COL17,'U')"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='FUTURE'">

								<Symbol>
									<xsl:value-of select="$varFutureSymbol"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL6,'&quot;','')"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>


							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="''"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
						</xsl:choose>

						<PBAssetName>
							<xsl:value-of select="COL2"/>
						</PBAssetName>



						<xsl:choose>
							<xsl:when test="boolean(number(COL9))">
								<AvgPX>
									<xsl:value-of select="number(COL9)"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>

						<PositionStartDate>
							<xsl:value-of select="COL7"/>
						</PositionStartDate>


						<xsl:choose>
							<xsl:when  test="boolean(number(COL8))">
								<Quantity>
									<xsl:value-of select="COL8"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test="number(COL8)&gt; 0 and COL2='EQUITY'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>

							<xsl:when test="number(COL8)&lt; 0  and COL2 ='EQUITY'">
								<Side>
									<xsl:value-of select="'Short Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="number(COL8)&gt; 0 and COL2 !='EQUITY'">
								<Side>
									<xsl:value-of select="'Buy to Open'"/>
								</Side>
							</xsl:when>
							<xsl:when test="number(COL8)&lt; 0 and COL2 !='EQUITY'">
								<Side>
									<xsl:value-of select="'Sell to open'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


