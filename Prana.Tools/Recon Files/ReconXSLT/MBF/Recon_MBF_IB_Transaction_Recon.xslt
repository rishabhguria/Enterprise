<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
				<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				
				<xsl:if test ="COL1 != 'ClientAccountID'">

					<PositionMaster>
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='IB']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="number(translate(COL11,',','')) &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="number(translate(COL11,',',''))*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="number(translate(COL11,',','')) &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="number(translate(COL11,',',''))"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<AvgPX>
							<xsl:value-of select="COL12"/>
						</AvgPX>

						<MarkPrice>
							<xsl:value-of select ="COL17"/>
						</MarkPrice>

						<!-- Symbol Section-->

						<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>

						<xsl:variable name="PB_COMPANY_NAME" select="COL5"/>

						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='IB']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="COL3='FUT'">
										<xsl:variable name = "varLength" >
											<xsl:value-of select="string-length(translate(translate(COL4,'&quot;',''),' ',''))"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test ="$varLength &gt; 0 ">
												<xsl:variable name = "varAfter" >
													<xsl:value-of select="substring(COL4,($varLength)-1,2)"/>
												</xsl:variable>
												<xsl:variable name = "varBefore" >
													<xsl:value-of select="substring(COL4,1,($varLength)-2)"/>
												</xsl:variable>
												<Symbol>
													<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
												</Symbol>
											</xsl:when>
											<xsl:otherwise>
												<Symbol>
													<xsl:value-of select="COL4"/>
												</Symbol>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="COL4"/>
										</Symbol>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>

						<!--GROSS NOTIONAL-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL14))">
								<xsl:choose>
									<xsl:when test="COL14 &lt; 0">
										<GrossNotionalValue>
											<xsl:value-of select="COL14*(-1)"/>
										</GrossNotionalValue>
									</xsl:when>
									<xsl:when test="COL14 &gt; 0">
										<GrossNotionalValue>
											<xsl:value-of select="COL14"/>
										</GrossNotionalValue>
									</xsl:when>
									<xsl:otherwise>
										<GrossNotionalValue>
											<xsl:value-of select="0"/>
										</GrossNotionalValue>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<GrossNotionalValue>
									<xsl:value-of select="0"/>
								</GrossNotionalValue>
							</xsl:otherwise>
						</xsl:choose>

						<!--COMMISSION-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL16))">
								<Commission>
									<xsl:value-of select="COL16*(-1)"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

					
						<xsl:choose>
							<xsl:when test ="boolean(number(COL15))">
								<Fees>
									<xsl:value-of select="COL15"/>
								</Fees>
							</xsl:when>
							<xsl:otherwise>
								<Fees>
									<xsl:value-of select="0"/>
								</Fees>
							</xsl:otherwise>
						</xsl:choose>

						<!--NET NOTIONAL-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL18))">
								<xsl:choose>
									<xsl:when test="COL18 &lt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL18*(-1)"/>
										</NetNotionalValue>
									</xsl:when>
									<xsl:when test="COL18 &gt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL18"/>
										</NetNotionalValue>
									</xsl:when>
									<xsl:otherwise>
										<NetNotionalValue>
											<xsl:value-of select="0"/>
										</NetNotionalValue>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValue>
									<xsl:value-of select="0"/>
								</NetNotionalValue>
							</xsl:otherwise>
						</xsl:choose>
					
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
