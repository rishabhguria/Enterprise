<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
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
	<xsl:template name="FormatDate">
		<xsl:param name="DateTime"/>
		<!--  converts date time double number to 18/12/2009  -->
		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019"/>
		</xsl:variable>
		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))"/>
		</xsl:variable>
		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))"/>
		</xsl:variable>
		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))"/>
		</xsl:variable>
		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31"/>
		</xsl:variable>
		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))"/>
		</xsl:variable>
		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))"/>
		</xsl:variable>
		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))"/>
		</xsl:variable>
		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))"/>
		</xsl:variable>
		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)"/>
		</xsl:variable>
		<xsl:variable name="varMonthUpdated">
			<xsl:choose>
				<xsl:when test="string-length($nMonth) = 1">
					<xsl:value-of select="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="nDayUpdated">
			<xsl:choose>
				<xsl:when test="string-length($nDay) = 1">
					<xsl:value-of select="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="$varMonthUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nDayUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nYear"/>
	</xsl:template>
	<xsl:variable name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:template match="/">
		
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL11"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varQuantity) and (contains(normalize-space(COL10),'Interest') or contains(normalize-space(COL10),'NORTHERN INSTL FDS') 
						or contains(normalize-space(COL10),'COST RECOVERY FOR MONTH ENDING') or contains(normalize-space(COL10),'MACS') 
						or contains(normalize-space(COL10),'DEL/REC') or contains(normalize-space(COL10),'INCOME RECEIVED') 
						or contains(normalize-space(COL10),'CSDR PEN') or contains(normalize-space(COL10),'ADR DEPOSITORY HOLDING CHARGES') or 
						contains(normalize-space(COL10),'Monthly Account Fee') or contains(normalize-space(COL10),'TRANSFER' or 'ADDITIONAL FUNDING'))">


					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME">
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<CurrencyName>
							<xsl:value-of select="normalize-space(COL5)"/>
						</CurrencyName>
												
						<xsl:variable name = "PB_COMPANY_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>														
									
						<xsl:variable name="varAmount">
							<xsl:choose>
								<xsl:when test="$varQuantity &gt;0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="$varQuantity &lt;0">
									<xsl:value-of select="$varQuantity*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_PRE_ACRONYM_NAME">
							<xsl:choose>
								<xsl:when test="contains(normalize-space(COL10),'Interest') or contains(normalize-space(COL10),'NORTHERN INSTL FDS') and $varQuantity &gt;0">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'Interest') or contains(normalize-space(COL10),'NORTHERN INSTL FDS') and $varQuantity &lt;0">
									<xsl:value-of select="'IntExpense'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'COST RECOVERY FOR MONTH ENDING') and $varQuantity &gt;0">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'COST RECOVERY FOR MONTH ENDING') and $varQuantity &lt;0">
									<xsl:value-of select="'IntExpense'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'MACS') or contains(normalize-space(COL10),'DEL/REC') or contains(normalize-space(COL10),'INCOME RECEIVED') or contains(normalize-space(COL10),'CSDR PEN') or contains(normalize-space(COL10),'ADR DEPOSITORY HOLDING CHARGES') and $varQuantity &gt;0">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'MACS') or contains(normalize-space(COL10),'DEL/REC') or contains(normalize-space(COL10),'INCOME RECEIVED') or contains(normalize-space(COL10),'CSDR PEN') or contains(normalize-space(COL10),'ADR DEPOSITORY HOLDING CHARGES') and $varQuantity &lt;0">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'Monthly Account Fee') and $varQuantity &gt;0">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'Monthly Account Fee') and $varQuantity &lt;0">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'TRANSFER' or 'ADDITIONAL FUNDING') and $varQuantity &gt;0">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'TRANSFER' or 'ADDITIONAL FUNDING') and $varQuantity &lt;0">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_POST_ACRONYM_NAME">
							<xsl:choose>
								<xsl:when test="contains(normalize-space(COL10),'Interest') or contains(normalize-space(COL10),'NORTHERN INSTL FDS') and $varQuantity &gt;0">
									<xsl:value-of select="'Intincome'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'NORTHERN INSTL FDS') and $varQuantity &lt;0">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'COST RECOVERY FOR MONTH ENDING') and $varQuantity &gt;0">
									<xsl:value-of select="'IntExpense'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'COST RECOVERY FOR MONTH ENDING') and $varQuantity &lt;0">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'MACS') or contains(normalize-space(COL10),'DEL/REC') or contains(normalize-space(COL10),'INCOME RECEIVED') or contains(normalize-space(COL10),'CSDR PEN') or contains(normalize-space(COL10),'ADR DEPOSITORY HOLDING CHARGES') and $varQuantity &gt;0">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'MACS') or contains(normalize-space(COL10),'DEL/REC') or contains(normalize-space(COL10),'INCOME RECEIVED') or contains(normalize-space(COL10),'CSDR PEN') or contains(normalize-space(COL10),'ADR DEPOSITORY HOLDING CHARGES') and $varQuantity &lt;0">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'Monthly Account Fee') and $varQuantity &gt;0">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'Monthly Account Fee') and $varQuantity &lt;0">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'TRANSFER' or 'ADDITIONAL FUNDING') and $varQuantity &gt;0">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(normalize-space(COL10),'TRANSFER' or 'ADDITIONAL FUNDING') and $varQuantity &lt;0">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<JournalEntries>													
						   <xsl:choose>
								<xsl:when test="$varQuantity &gt;0">
							   <xsl:value-of select="concat($PRANA_PRE_ACRONYM_NAME, ':' , $varQuantity , '|' , $PRANA_POST_ACRONYM_NAME, ':' , $varQuantity)"/>
								</xsl:when>
								<xsl:when test="$varQuantity &lt;0">
							   <xsl:value-of select="concat($PRANA_POST_ACRONYM_NAME, ':' , $varQuantity , '|' , $PRANA_PRE_ACRONYM_NAME, ':' , $varQuantity)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</JournalEntries>

						<FXRate>
							<xsl:value-of select="0"/>
						</FXRate>

						<Date>
							<xsl:call-template name="FormatDate">
								<xsl:with-param name="DateTime" select="COL8"/>
							</xsl:call-template>
						</Date>
						
						<xsl:variable name="varDescriptionName">
							<xsl:value-of select="COL10"/>
						</xsl:variable>

						<Description>
							<xsl:value-of select="$varDescriptionName"/>
						</Description>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>