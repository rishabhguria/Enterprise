<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL5)">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'MS'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_SUFFIX_CODE" >
							<xsl:value-of select ="normalize-space(substring-after(COL2,' '))"/>
						</xsl:variable>

						<xsl:variable name = "PB_PREFIX_CODE" >
							<xsl:value-of select ="substring-before(COL2,' ')"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
						</xsl:variable>
						
						<xsl:variable name = "PB_CODE" >
							<xsl:value-of select ="substring(COL2,1,2)"/>
						</xsl:variable>


						<xsl:variable name ="PRANA_UNDERLYING_CODE">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE]/@UnderlyingCode"/>
						</xsl:variable>
						
						<xsl:variable name ="PRANA_EXCHNGE_CODE">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_CODE]/@ExchangeName"/>
						</xsl:variable>

						<xsl:variable name="HKG">
							<xsl:choose>
								
								<xsl:when test ="$PB_SUFFIX_CODE = 'HK'">									
									<xsl:choose>
										<xsl:when test="string-length($PB_PREFIX_CODE) =1">
											<xsl:value-of select="concat('000',$PB_PREFIX_CODE,'-HKG')"/>
										</xsl:when>
										<xsl:when test="string-length($PB_PREFIX_CODE)=2">
											<xsl:value-of select="concat('00',$PB_PREFIX_CODE,'-HKG')"/>
										</xsl:when>
										<xsl:when test="string-length($PB_PREFIX_CODE) =3">
											<xsl:value-of select="concat('0',$PB_PREFIX_CODE,'-HKG')"/>
										</xsl:when>
										<xsl:when test="string-length($PB_PREFIX_CODE) =4">
											<xsl:value-of select="concat($PB_PREFIX_CODE,'-HKG')"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								
							</xsl:choose>
						</xsl:variable>
						

						<Symbol>

							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test ="$PB_SUFFIX_CODE = 'HK'">
									<xsl:value-of select ="$HKG"/>
								</xsl:when>


								<xsl:when test ="$PRANA_SUFFIX_NAME != ''">
									<xsl:value-of select ="concat(substring-before(COL2,' '),$PRANA_SUFFIX_NAME)"/>
								</xsl:when>



								<xsl:when test="COL11='FUTURE'">
									<xsl:choose>

										<xsl:when test ="$PRANA_UNDERLYING_CODE != ''">
											<xsl:value-of select ="concat($PRANA_UNDERLYING_CODE,' ',substring(COL2,3,2),$PRANA_EXCHNGE_CODE)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="normalize-space(concat(substring(COL2,1,2),' ',substring(COL2,3,2)))"/>
										</xsl:otherwise>

									</xsl:choose>
									
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="COL2"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>


						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="'Asiya'"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<PositionStartDate>
							<xsl:value-of select ="''"/>
						</PositionStartDate>


						<xsl:variable name ="NetPosition">
							<xsl:value-of select ="number(COL5)"/>
						</xsl:variable>

						<NetPosition>

							<xsl:choose>

								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="$NetPosition*-1"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetPosition>
						
						<xsl:variable name="varside" select="COL1"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varside='S'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varside='B'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varside='BC'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$varside='SS'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>


						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<TradeAttribute1>
							<xsl:choose>
								<xsl:when test="contains(COL11,'SWAP')">
									<xsl:choose>

										<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
											<xsl:value-of select ="translate(concat($PRANA_SYMBOL_NAME,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:when>

										<xsl:when test ="$PB_SUFFIX_CODE = 'HK'">
											<xsl:value-of select ="translate(concat($HKG,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:when>


										<xsl:when test ="$PRANA_SUFFIX_NAME != ''">
											<xsl:value-of select ="translate(concat(substring-before(COL2,' '),$PRANA_SUFFIX_NAME,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:when>



										<xsl:when test="COL11='FUTURE'">
											<xsl:choose>

												<xsl:when test ="$PRANA_UNDERLYING_CODE != ''">
													<xsl:value-of select ="translate(concat($PRANA_UNDERLYING_CODE,' ',substring(COL2,3,2),$PRANA_EXCHNGE_CODE,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
												</xsl:when>

												<xsl:otherwise>
													<xsl:value-of select="translate(normalize-space(concat(substring(COL2,1,2),' ',substring(COL2,3,2),'_swap')),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
												</xsl:otherwise>

											</xsl:choose>

										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="translate(concat(COL2,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
											<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
										</xsl:when>

										<xsl:when test ="$PB_SUFFIX_CODE = 'HK'">
											<xsl:value-of select ="$HKG"/>
										</xsl:when>


										<xsl:when test ="$PRANA_SUFFIX_NAME != ''">
											<xsl:value-of select ="concat(substring-before(COL2,' '),$PRANA_SUFFIX_NAME)"/>
										</xsl:when>



										<xsl:when test="COL11='FUTURE'">
											<xsl:choose>

												<xsl:when test ="$PRANA_UNDERLYING_CODE != ''">
													<xsl:value-of select ="concat($PRANA_UNDERLYING_CODE,' ',substring(COL2,3,2),$PRANA_EXCHNGE_CODE)"/>
												</xsl:when>

												<xsl:otherwise>
													<xsl:value-of select="normalize-space(concat(substring(COL2,1,2),' ',substring(COL2,3,2)))"/>
												</xsl:otherwise>

											</xsl:choose>

										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="COL2"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</TradeAttribute1>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
