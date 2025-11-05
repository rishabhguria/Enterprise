<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template name="left-trim">
		<xsl:param name="s" />
		<xsl:choose>
			<xsl:when test="substring($s, 1, 1) = ''">
				<xsl:value-of select="$s"/>
			</xsl:when>
			<xsl:when test="normalize-space(substring($s, 1, 1)) = ''">
				<xsl:call-template name="left-trim">
					<xsl:with-param name="s" select="substring($s, 2)" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$s" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="right-trim">
		<xsl:param name="s" />
		<xsl:choose>
			<xsl:when test="substring($s, 1, 1) = ''">
				<xsl:value-of select="$s"/>
			</xsl:when>
			<xsl:when test="normalize-space(substring($s, string-length($s))) = ''">
				<xsl:call-template name="right-trim">
					<xsl:with-param name="s" select="substring($s, 1, string-length($s) - 1)" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$s" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="trim">
		<xsl:param name="s" />
		<xsl:call-template name="right-trim">
			<xsl:with-param name="s">
				<xsl:call-template name="left-trim">
					<xsl:with-param name="s" select="$s" />
				</xsl:call-template>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>	
	
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:variable name = "PB_ASSET_NAME" >
					<xsl:value-of select="translate(COL2,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="$PB_ASSET_NAME != 'Investment Type' and $PB_ASSET_NAME != 'CURRENCY' and COL8 != 0 and COL8 != 'QUANTITY'">
					<PositionMaster>

						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GSEC']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
						</xsl:choose>
						
						<!--  Symbol Region -->
						<xsl:variable name = "varInstrumentType" >
							<xsl:value-of select="translate(translate(COL2, ' ' , ''),'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PB_COMPANY_NAME">
							<xsl:call-template name="trim">
								<xsl:with-param name="s" select="translate(COL5,'&quot;','')" />
							</xsl:call-template>
						</xsl:variable> 
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSEC']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL_NAME = ''">
								<xsl:choose>
									<xsl:when test ="$varInstrumentType='EQUITY'">
										<Symbol>
											<xsl:call-template name="trim">
												<xsl:with-param name="s" select="translate(COL4,'&quot;','')" />
											</xsl:call-template>
										</Symbol>
										<PBSymbol>
											<xsl:value-of select="translate(COL4,'&quot;','')"/>
										</PBSymbol>
									</xsl:when>
									<xsl:when test ="$varInstrumentType='OPTION'">
										<Symbol>
											<xsl:call-template name="trim">
												<xsl:with-param name="s" select="translate(COL6,'&quot;','')" />
											</xsl:call-template>
										</Symbol>
										<PBSymbol>
											<xsl:value-of select="translate(COL6,'&quot;','')"/>
										</PBSymbol>
									</xsl:when>
									<xsl:when test ="$varInstrumentType='FUTURE'">
										<xsl:variable name = "varLength" >
											<xsl:value-of select="string-length(translate(translate(COL7,'&quot;',''),' ',''))"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test ="$varLength &gt; 0 ">
												<xsl:variable name = "varAfter" >
													<xsl:value-of select="substring(COL7,($varLength)-1,2)"/>
												</xsl:variable>
												<xsl:variable name = "varBefore" >
													<xsl:value-of select="substring(COL7,1,($varLength)-2)"/>
												</xsl:variable>
												<Symbol>
													<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
												</Symbol>
												<PBSymbol>
													<xsl:value-of select="translate(COL7,'&quot;','')"/>
												</PBSymbol>
											</xsl:when>
											<xsl:otherwise>
												<Symbol>
													<xsl:value-of select="''"/>
												</Symbol>
												<PBSymbol>
													<xsl:value-of select="''"/>
												</PBSymbol>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>
										<PBSymbol>
											<xsl:value-of select="''"/>
										</PBSymbol>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</PBSymbol>
							</xsl:otherwise>
						</xsl:choose>
						
						
						<xsl:choose>
							<xsl:when test="COL8 &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL8*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test="COL8 &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL8"/>
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

						<xsl:choose>
							<xsl:when test ="boolean(number(COL11))">
								<AvgPX>
									<xsl:value-of select="COL11"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>						

						<!--AssetName section-->
						<xsl:variable name="PRANA_ASSET_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='GSEC']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_ASSET_NAME=''">
								<PBAssetName>
									<xsl:value-of select='$PB_ASSET_NAME'/>
								</PBAssetName>
							</xsl:when>
							<xsl:otherwise>
								<PBAssetName>
									<xsl:value-of select='$PRANA_ASSET_NAME'/>
								</PBAssetName>
							</xsl:otherwise>
						</xsl:choose>


						<!--GROSS NOTIONAL-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL12))">
								<xsl:choose>
									<xsl:when test="COL12 &lt; 0">
										<GrossNotionalValue>
											<xsl:value-of select="COL12*(-1)"/>
										</GrossNotionalValue>
									</xsl:when>
									<xsl:when test="COL12 &gt; 0">
										<GrossNotionalValue>
											<xsl:value-of select="COL12"/>
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
							<xsl:when test ="boolean(number(COL13))">
								<Commission>
									<xsl:value-of select="COL13*(-1)"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<!--COST BASIS-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL16))">
								<Fees>
									<xsl:value-of select="(COL16 + COL17) * (-1)"/>
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
							<xsl:when test ="boolean(number(COL19))">
								<xsl:choose>
									<xsl:when test="COL19 &lt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL19*(-1)"/>
										</NetNotionalValue>
									</xsl:when>
									<xsl:when test="COL19 &gt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL19"/>
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
