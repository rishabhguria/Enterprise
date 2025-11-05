<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
                xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>
	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 'Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month = 'May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01' ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02' ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03' ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04' ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05' ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06' ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07'  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09' ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10' ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11' ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12' ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01' ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02' ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03' ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04' ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05' ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06' ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09' ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10' ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11' ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12' ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="normalize-space(COL27)='OPTION'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before($Symbol,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after($Symbol,' '),3,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after($Symbol,' '),5,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after($Symbol,' '),1,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after($Symbol,' '),7,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after($Symbol,' '),8,5),'#.00')"/>
			</xsl:variable>

			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

					<xsl:choose>
						<xsl:when test="substring(substring-after($ThirdFriday,'/'),1,2) = ($ExpiryDay - 1)">
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL19"/>
					</xsl:call-template>
				</xsl:variable>



				<xsl:if test="number($Position) and (COL23='BL' or COL23='SL' or COL23='SS' or COL23='STO' or COL23='STC' or COL23='CS')">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Scotiabank'"/>
						</xsl:variable>
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL17"/>
						</xsl:variable>
				
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						

						<xsl:variable name="PB_SUFFIX_NAME" select="normalize-space(COL8)"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@TickerSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
									<xsl:value-of select="normalize-space(COL18)"/>	
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:variable name="AssetType">
							<xsl:choose>
								<xsl:when test="normalize-space(COL27)='OPTION'">
									<xsl:value-of select="'Option'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$AssetType='Option'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Suffix" select="$PRANA_SUFFIX_NAME"/>
										<xsl:with-param name="Symbol" select="normalize-space(translate($varSymbol,'.',''))"/>
									</xsl:call-template>
									<!--<xsl:value-of select="''"/>-->
								</xsl:when>
								<xsl:when test="contains(COL27,'CORP') or contains(COL27,'CONV BOND')">
									<xsl:value-of select="COL16"/>
								</xsl:when>
								<xsl:when test="substring-after(COL22,' ')='CV'">
									<xsl:value-of select="concat(substring-before(COL22,' '),'-VC')"/>
								</xsl:when>
								<xsl:when test="$AssetType='Equity'">
									<xsl:choose>
										<xsl:when test="COL8='CAD'">
											<xsl:choose>
												<xsl:when test="contains($varSymbol,'/')">
													<xsl:value-of select="concat(translate($varSymbol,'/','.'),$PRANA_SUFFIX_NAME)"/>
												</xsl:when>
												<xsl:when test="contains($varSymbol,'-')">
													<xsl:value-of select="concat(translate($varSymbol,'-','.'),'N',$PRANA_SUFFIX_NAME)"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:choose>
														<xsl:when test="contains(substring($varSymbol,string-length($varSymbol)-1),'CT')">
															<xsl:value-of select ="concat(substring($varSymbol,1,string-length($varSymbol)-2),$PRANA_SUFFIX_NAME)"/>
														</xsl:when>
														<xsl:when test="contains(substring($varSymbol,string-length($varSymbol)-1),'US')">
															<xsl:value-of select="substring($varSymbol,1,string-length($varSymbol)-3)"/>
														</xsl:when>
														<xsl:otherwise>
															<!--<xsl:value-of select ="normalize-space($varSymbol)"/>-->
															<!--<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>-->
														</xsl:otherwise>
													</xsl:choose>
													<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="contains(substring($varSymbol,string-length($varSymbol)-1),'CN') or contains(substring($varSymbol,string-length($varSymbol)-1),'CT')">
											<xsl:value-of select ="concat(substring($varSymbol,1,string-length($varSymbol)-2),'-TC')"/>
										</xsl:when>
										<xsl:when test="contains(substring($varSymbol,string-length($varSymbol)-1),'US')">
											<xsl:value-of select="substring($varSymbol,1,string-length($varSymbol)-2)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varSymbol"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

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



						<ExecutingBroker>
							<xsl:value-of select="normalize-space(COL13)"/>
						</ExecutingBroker>
						<xsl:variable name="AccruedInterest">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL33"/>
							</xsl:call-template>
						</xsl:variable>

						<AccruedInterest>
							<xsl:choose>
								<xsl:when test ="$AccruedInterest!=''">
									<xsl:value-of select ="$AccruedInterest"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
							
						</AccruedInterest>

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

						<xsl:variable name="Side" select="normalize-space(COL23)"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Side='BL'">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when test="$Side='SL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>

								<xsl:when test="$Side='SS'">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:when test="$Side='CS'">
									<xsl:value-of select="'B'"/>
								</xsl:when>

								<xsl:when test="$Side='STO'">
									<xsl:value-of select="'C'"/>
								</xsl:when>

								<xsl:when test="$Side='STC'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name="Principal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL36"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Cost" select="COL24"/>

						<CostBasis>
							<xsl:choose>
								<xsl:when test="$Cost &gt; 0">
									<xsl:value-of select="$Cost"/>
								</xsl:when>
								<xsl:when test="$Cost &lt; 0">
									<xsl:value-of select="$Cost*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varCoupon">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>
						<Coupon>
							<xsl:value-of select="$varCoupon"/>
						</Coupon>

						<Strategy>
							<xsl:value-of select="normalize-space(COL11)"/>
						</Strategy>
						
						<xsl:variable name="Month1">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL5,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<PositionStartDate>
							<xsl:choose>
								<xsl:when test="contains(substring-after(substring-after(normalize-space(COL5),'-'),'-'),'20')">
									<xsl:value-of select="concat($Month1,'/',substring-before(normalize-space(COL5),'-'),'/',substring-after(substring-after(COL5,'-'),'-'))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="concat($Month1,'/',substring-before(normalize-space(COL5),'-'),'/',20,substring-after(substring-after(COL5,'-'),'-'))"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionStartDate>

						<xsl:variable name="Month2">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL6,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<PositionSettlementDate>
							<xsl:choose>
								<xsl:when test="contains(substring-after(substring-after(normalize-space(COL6),'-'),'-'),'20')">
									<xsl:value-of select="concat($Month2,'/',substring-before(normalize-space(COL6),'-'),'/',substring-after(substring-after(COL6,'-'),'-'))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="concat($Month2,'/',substring-before(normalize-space(COL6),'-'),'/',20,substring-after(substring-after(COL6,'-'),'-'))"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionSettlementDate>


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL25"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>						
								
									<xsl:choose>
										<xsl:when test="$Commission &gt; 0">
											<xsl:value-of select="$Commission"/>
										</xsl:when>
										<xsl:when test="$Commission &lt; 0">
											<xsl:value-of select="$Commission*-1"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								
						</Commission>

						<xsl:variable name="ExchangFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL29 + COL31"/>
							</xsl:call-template>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test="$ExchangFee &gt; 0">
									<xsl:value-of select="$ExchangFee"/>
								</xsl:when>
								<xsl:when test="$ExchangFee &lt; 0">
									<xsl:value-of select="$ExchangFee*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>
						<xsl:variable name="SecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30"/>
							</xsl:call-template>
						</xsl:variable>

						<SecFee>
							<xsl:choose>
								<xsl:when test="$SecFee &gt; 0">
									<xsl:value-of select="$SecFee"/>
								</xsl:when>
								<xsl:when test="$SecFee &lt; 0">
									<xsl:value-of select="$SecFee*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<xsl:variable name="OrfFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL32"/>
							</xsl:call-template>
						</xsl:variable>

						<OrfFee>
							<xsl:choose>
								<xsl:when test="$OrfFee &gt; 0">
									<xsl:value-of select="$OrfFee"/>
								</xsl:when>
								<xsl:when test="$OrfFee &lt; 0">
									<xsl:value-of select="$OrfFee*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrfFee>


						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
