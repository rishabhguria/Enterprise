<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

  <xsl:key name="col" match="DocumentElement/RASImport[3]/*" use="name()" /> 
    <xsl:template name="Translate">
        <xsl:param name="Number"/>
        <xsl:variable name="SingleQuote">'</xsl:variable>
        <xsl:variable name="Quote">&amp;</xsl:variable>
        <xsl:variable name="varNumber">
            <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),$Quote,''))"/>
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
	
    <xsl:template match="/DocumentElement">

    <xsl:variable name = "varBPS">
      <xsl:value-of select="RASImport[COL2 != ''][COL2 = 'BPS']/COL2"/>
    </xsl:variable>
    <xsl:variable name = "varPercentage">
      <xsl:value-of select="RASImport[COL2 != ''][COL2 = 'Percentage']/COL2"/>
    </xsl:variable>

    <xsl:copy>
      <xsl:for-each select="RASImport[position() > 3]">
        <xsl:variable name="varSymbol" select="COL1" />
        
        <xsl:for-each select="*[not(self::COL1)]">

        <xsl:variable name="varQuantity">
			<xsl:call-template name="Translate">
				<xsl:with-param name="Number" select="."/>
			</xsl:call-template>
        </xsl:variable>            
           
        <xsl:variable name="varQuantitySet">
            <xsl:value-of select="."/>
          </xsl:variable>
			<xsl:variable name ="varGroup_Account">
				<xsl:value-of select="normalize-space(key('col', name()))"/>
			</xsl:variable>

			<xsl:if test="((number($varQuantity) or contains($varQuantitySet,'&amp;')) and $varQuantity !='*') and $varGroup_Account = 'Select'">
				<RASImport>

              <Symbol>
                <xsl:value-of select="$varSymbol"/>
              </Symbol>            

              <AccountOrGroupName>
                <xsl:value-of select="'NZS Select'"/>
              </AccountOrGroupName>
			
              <IncreaseDecreaseOrSet>
                <xsl:choose>
                  <xsl:when test="contains($varQuantitySet,'&amp;')">
                    <xsl:value-of select="'Set'"/>
                  </xsl:when>
                  <xsl:when test="contains($varQuantitySet,'&amp;') and number($varQuantity) &lt; 0">
                    <xsl:value-of select="'Set'"/>
                  </xsl:when>
                    <xsl:when test="number($varQuantity) and number($varQuantity) &lt; 0">
                        <xsl:value-of select="'Decrease'"/>
                    </xsl:when>
                    <xsl:when test="number($varQuantity) and number($varQuantity) &gt; 0">
                        <xsl:value-of select="'Increase'"/>
                    </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </IncreaseDecreaseOrSet>			
				
              <BPSOrPercentage>
                <xsl:choose>
                  <xsl:when test="$varBPS != ''">
                    <xsl:value-of select="$varBPS"/>
                  </xsl:when>
                  <xsl:when test="$varPercentage != ''">
                    <xsl:value-of select="$varPercentage"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </BPSOrPercentage>

              <Target>
                <xsl:value-of select="$varQuantity"/>
              </Target>

            </RASImport>
			</xsl:if>

			<xsl:if test="((number($varQuantity) or contains($varQuantitySet,'&amp;')) and $varQuantity !='*') and $varGroup_Account = 'Growth Equity'">
				<RASImport>

				  <Symbol>
					<xsl:value-of select="$varSymbol"/>
				  </Symbol>

				  <AccountOrGroupName>
					<xsl:value-of select="'Cassini Partners LP MIT'"/>
				  </AccountOrGroupName>

				  <IncreaseDecreaseOrSet>
					<xsl:choose>
					  <xsl:when test="contains($varQuantitySet,'&amp;')">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
					  <xsl:when test="contains($varQuantitySet,'&amp;') and number($varQuantity) &lt; 0">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &lt; 0">
							<xsl:value-of select="'Decrease'"/>
						</xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &gt; 0">
							<xsl:value-of select="'Increase'"/>
						</xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
      			  </IncreaseDecreaseOrSet>
			  
				  <BPSOrPercentage>
					<xsl:choose>
					  <xsl:when test="$varBPS != ''">
						<xsl:value-of select="$varBPS"/>
					  </xsl:when>
					  <xsl:when test="$varPercentage != ''">
						<xsl:value-of select="$varPercentage"/>
					  </xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </BPSOrPercentage>

				  <Target>
					<xsl:value-of select="$varQuantity"/>
				  </Target>

				</RASImport>

				<RASImport>

				  <Symbol>
					<xsl:value-of select="$varSymbol"/>
				  </Symbol>

				  <AccountOrGroupName>
					<xsl:value-of select="'238 Plan Associates LLC MIT'"/>
				  </AccountOrGroupName>

                  <IncreaseDecreaseOrSet>
					<xsl:choose>
					  <xsl:when test="contains($varQuantitySet,'&amp;')">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
					  <xsl:when test="contains($varQuantitySet,'&amp;') and number($varQuantity) &lt; 0">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &lt; 0">
							<xsl:value-of select="'Decrease'"/>
						</xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &gt; 0">
							<xsl:value-of select="'Increase'"/>
						</xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </IncreaseDecreaseOrSet>

				  <BPSOrPercentage>
					<xsl:choose>
					  <xsl:when test="$varBPS != ''">
						<xsl:value-of select="$varBPS"/>
					  </xsl:when>
					  <xsl:when test="$varPercentage != ''">
						<xsl:value-of select="$varPercentage"/>
					  </xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </BPSOrPercentage>

				  <Target>
					<xsl:value-of select="$varQuantity"/>
				  </Target>

				</RASImport>

				<RASImport>

				  <Symbol>
					<xsl:value-of select="$varSymbol"/>
				  </Symbol>

				  <AccountOrGroupName>
					<xsl:value-of select="'NBIM PF EX EQ NZS GBL T EX CH'"/>
				  </AccountOrGroupName>

				  <IncreaseDecreaseOrSet>
					<xsl:choose>
					  <xsl:when test="contains($varQuantitySet,'&amp;')">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
					  <xsl:when test="contains($varQuantitySet,'&amp;') and number($varQuantity) &lt; 0">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &lt; 0">
							<xsl:value-of select="'Decrease'"/>
						</xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &gt; 0">
							<xsl:value-of select="'Increase'"/>
						</xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </IncreaseDecreaseOrSet>
					
				  <BPSOrPercentage>
					<xsl:choose>
					  <xsl:when test="$varBPS != ''">
						<xsl:value-of select="$varBPS"/>
					  </xsl:when>
					  <xsl:when test="$varPercentage != ''">
						<xsl:value-of select="$varPercentage"/>
					  </xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </BPSOrPercentage>

				  <Target>
					<xsl:value-of select="$varQuantity"/>
				  </Target>

				</RASImport>
				
			</xsl:if>
          
			<xsl:if test="((number($varQuantity) or contains($varQuantitySet,'&amp;')) and $varQuantity !='*') and $varGroup_Account = 'Technology'">
				<RASImport>

				  <Symbol>
					<xsl:value-of select="$varSymbol"/>
				  </Symbol>            

				  <AccountOrGroupName>
					<xsl:value-of select="'CHALLENGE TECHNOLOGY EQ EVOLUTION3'"/>
				  </AccountOrGroupName>

                  <IncreaseDecreaseOrSet>
					<xsl:choose>
					  <xsl:when test="contains($varQuantitySet,'&amp;')">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
					  <xsl:when test="contains($varQuantitySet,'&amp;') and number($varQuantity) &lt; 0">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &lt; 0">
							<xsl:value-of select="'Decrease'"/>
						</xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &gt; 0">
							<xsl:value-of select="'Increase'"/>
						</xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </IncreaseDecreaseOrSet>
					
				  <BPSOrPercentage>
					<xsl:choose>
					  <xsl:when test="$varBPS != ''">
						<xsl:value-of select="$varBPS"/>
					  </xsl:when>
					  <xsl:when test="$varPercentage != ''">
						<xsl:value-of select="$varPercentage"/>
					  </xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </BPSOrPercentage>

				  <Target>
					<xsl:value-of select="$varQuantity"/>
				  </Target>

				</RASImport>
			</xsl:if>
        
			<xsl:if test="((number($varQuantity) or contains($varQuantitySet,'&amp;')) and $varQuantity !='*') and $varGroup_Account = 'Global Growth Equity'">
				<RASImport>

				  <Symbol>
					<xsl:value-of select="$varSymbol"/>
				  </Symbol>            

				  <AccountOrGroupName>
					<xsl:value-of select="'JUPITER NZS GLO EQ GR UNCONSTR Delaware'"/>
				  </AccountOrGroupName>
                  
				  <IncreaseDecreaseOrSet>
					<xsl:choose>
					  <xsl:when test="contains($varQuantitySet,'&amp;')">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
					  <xsl:when test="contains($varQuantitySet,'&amp;') and number($varQuantity) &lt; 0">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &lt; 0">
							<xsl:value-of select="'Decrease'"/>
						</xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &gt; 0">
							<xsl:value-of select="'Increase'"/>
						</xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </IncreaseDecreaseOrSet>
					
				  <BPSOrPercentage>
					<xsl:choose>
					  <xsl:when test="$varBPS != ''">
						<xsl:value-of select="$varBPS"/>
					  </xsl:when>
					  <xsl:when test="$varPercentage != ''">
						<xsl:value-of select="$varPercentage"/>
					  </xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </BPSOrPercentage>

				  <Target>
					<xsl:value-of select="$varQuantity"/>
				  </Target>

				</RASImport>
				
				<RASImport>

				  <Symbol>
					<xsl:value-of select="$varSymbol"/>
				  </Symbol>            

				  <AccountOrGroupName>
					<xsl:value-of select="'JUPITER NZS GLO EQ GR UNCONSTR SICAV'"/>
				  </AccountOrGroupName>
				  
                  <IncreaseDecreaseOrSet>
					<xsl:choose>
					  <xsl:when test="contains($varQuantitySet,'&amp;')">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
					  <xsl:when test="contains($varQuantitySet,'&amp;') and number($varQuantity) &lt; 0">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &lt; 0">
							<xsl:value-of select="'Decrease'"/>
						</xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &gt; 0">
							<xsl:value-of select="'Increase'"/>
						</xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </IncreaseDecreaseOrSet>
					
				  <BPSOrPercentage>
					<xsl:choose>
					  <xsl:when test="$varBPS != ''">
						<xsl:value-of select="$varBPS"/>
					  </xsl:when>
					  <xsl:when test="$varPercentage != ''">
						<xsl:value-of select="$varPercentage"/>
					  </xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </BPSOrPercentage>

				  <Target>
					<xsl:value-of select="$varQuantity"/>
				  </Target>

				</RASImport>
				
				<RASImport>

				  <Symbol>
					<xsl:value-of select="$varSymbol"/>
				  </Symbol>            

				  <AccountOrGroupName>
					<xsl:value-of select="'Jpool 2'"/>
				  </AccountOrGroupName>

				  <IncreaseDecreaseOrSet>
					<xsl:choose>
					  <xsl:when test="contains($varQuantitySet,'&amp;')">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
					  <xsl:when test="contains($varQuantitySet,'&amp;') and number($varQuantity) &lt; 0">
						<xsl:value-of select="'Set'"/>
					  </xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &lt; 0">
							<xsl:value-of select="'Decrease'"/>
						</xsl:when>
						<xsl:when test="number($varQuantity) and number($varQuantity) &gt; 0">
							<xsl:value-of select="'Increase'"/>
						</xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </IncreaseDecreaseOrSet>

				  <BPSOrPercentage>
					<xsl:choose>
					  <xsl:when test="$varBPS != ''">
						<xsl:value-of select="$varBPS"/>
					  </xsl:when>
					  <xsl:when test="$varPercentage != ''">
						<xsl:value-of select="$varPercentage"/>
					  </xsl:when>
					  <xsl:otherwise>
						<xsl:value-of select="''"/>
					  </xsl:otherwise>
					</xsl:choose>
				  </BPSOrPercentage>

				  <Target>
					<xsl:value-of select="$varQuantity"/>
				  </Target>

				</RASImport>
			</xsl:if>
		</xsl:for-each>
      </xsl:for-each>
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>